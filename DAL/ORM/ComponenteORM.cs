using DAL.ORM.BASE;
using ENTITY;
using MAPPER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DAL.ORM
{
    /* -----------------------------------------------------------------------------------------------------
     * Clase: ComponenteORM
     * Descripción: ORM concreto para la entidad Componente (Leaf y Composite). Hereda de
     *              OrmXmlBase<Componente> y delega la conversión XML ↔ objeto al ComponenteMapper.
     *              Los datos se persisten en el archivo "componentes.xml".
     *
     *              A diferencia de los demás ORM, al leer arma el árbol: el XML guarda en cada Composite
     *              solo los Id de sus hijos, y este ORM los convierte en objetos (ResolverHijos).
     *              Para un mismo Id siempre se usa la misma instancia, por lo que un Composite reutilizado
     *              por varios roles existe una sola vez en memoria.
     -----------------------------------------------------------------------------------------------------*/

    public class ComponenteORM : OrmXmlBase<Componente>
    {
        /* Nombre del archivo donde se guardan todos los componentes (Leaf y Composite juntos) */
        protected override string NombreArchivo => "componentes.xml";

        /* Nombre que lleva cada nodo dentro del archivo: <Componente> ... </Componente> */
        protected override string NombreElemento => "Componente";

        /* Cómo pasar de un nodo XML a un objeto: lo hace el ComponenteMapper */
        protected override Componente MapearDesdeXml(XElement nodo) => ComponenteMapper.Map(nodo);

        /* Cómo pasar de un objeto a un nodo XML: lo hace el ComponenteMapper */
        protected override XElement MapearAXml(Componente entidad) => ComponenteMapper.ToXml(entidad);

        /* -----------------------------------------------------------------------------------------------------
         * Función: ObtenerTodos
         * Descripción: Lee todos los componentes del XML y arma las relaciones entre ellos.
         *              ObtenerPorId, Insertar y Actualizar de la clase base usan este método, por lo que
         *              también trabajan siempre con el árbol ya armado.
         -----------------------------------------------------------------------------------------------------*/
        public override List<Componente> ObtenerTodos()
        {
            /* Lee los nodos del XML con el método de la clase base.
               Los Composite llegan solo con IdsHijos (los números), todavía sin los objetos hijos */
            List<Componente> componentes = base.ObtenerTodos();

            /* Convierte esos números en los objetos reales dentro de cada Composite */
            ResolverHijos(componentes);

            return componentes;
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: Eliminar
         * Descripción: Elimina un componente y además lo quita de todos los Composite que lo contenían,
         *              para que no queden Id huérfanos en el XML.
         -----------------------------------------------------------------------------------------------------*/
        public override void Eliminar(int id)
        {
            /* Bloquea el acceso al archivo para que nadie más lo modifique mientras se elimina */
            lock (CandadoPersistencia)
            {
                /* Lee todos los componentes con el árbol armado */
                List<Componente> lista = ObtenerTodos();

                /* Busca el componente a eliminar. Si no existe, no hay nada que hacer */
                Componente eliminado = lista.FirstOrDefault(c => c.Id == id);
                if (eliminado == null)
                    return;

                /* Recorre solo los Composite (los grupos) y lo saca de cualquiera que lo tenga como hijo. */
                foreach (Composite compuesto in lista.OfType<Composite>())
                    compuesto.Delete(eliminado);

                /* Lo elimina de la lista y guarda el archivo actualizado */
                lista.Remove(eliminado);
                GuardarLista(lista);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: ResolverHijos
         * Descripción: Para cada Composite busca por Id a sus hijos y los agrega a su lista Hijos.
         *              Si un Id no existe, o el enlace generaría una referencia circular, lo ignora.
         *              Así un XML inconsistente no puede provocar un bucle infinito al recorrer el árbol.
         -----------------------------------------------------------------------------------------------------*/
        private void ResolverHijos(List<Componente> componentes)
        {
            /* Arma un diccionario para encontrar cualquier componente por su Id al instante.
               Como cada Id apunta a un único objeto, todos los grupos que usen el mismo hijo
               comparten el mismo objeto en memoria (no se hacen copias) */
            Dictionary<int, Componente> porId = componentes.ToDictionary(c => c.Id);

            /* Recorre solo los Composite: los Leaf no tienen hijos para resolver */
            foreach (Composite compuesto in componentes.OfType<Composite>())
            {
                /* Empieza con la lista de hijos vacía */
                compuesto.Hijos.Clear();

                /* Por cada número guardado en el XML, busca el objeto que le corresponde */
                foreach (int idHijo in compuesto.IdsHijos)
                {
                    /* Si ese Id ya no existe (por ejemplo se eliminó), lo salta */
                    if (!porId.TryGetValue(idHijo, out Componente hijo))
                        continue;

                    try
                    {
                        /* Agrega el hijo al grupo. Insert() controla que no haya círculos ni duplicados */
                        compuesto.Insert(hijo);
                    }
                    catch (Exception)
                    {
                        /* Si el enlace es inválido (círculo o repetido) se ignora este hijo.
                           Esto protege al programa de un XML mal armado. */
                    }
                }
            }
        }
    }
}
}
