using ENTITY;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DAL.ORM.BASE
{
    /* -----------------------------------------------------------------------------------------------------
     * Clase: OrmXmlBase<T>
     * Descripción: Clase base abstracta y genérica que implementa todas las operaciones de 
     *              persistencia sobre archivos XML. 
     *              Aplica el patrón Template Method:
     *              Define el algoritmo general y delega los detalles específicos a las 
     *              subclases mediante métodos abstractos.
     *              
     *              Al heredar de esta clase, cada ORM concreto solo necesita indicar:
     *              - NombreArchivo  → nombre del archivo XML (ej: "clientes.xml")
     *              - NombreElemento → nombre del nodo XML   (ej: "Cliente")
     *              - MapearDesdeXml → cómo convertir XElement a objeto
     *              - MapearAXml     → cómo convertir objeto a XElement
     *              
     *              Usa lock(CandadoPersistencia) para evitar problemas si dos operaciones
     *              intentan escribir el mismo archivo al mismo tiempo.
     -----------------------------------------------------------------------------------------------------*/
    public abstract class OrmXmlBase<T> : IOrmGenerico<T> where T : class, IEntidadConId
    {
        /* Objeto para evitar accesos simultáneos al archivo XML */
        protected static readonly object CandadoPersistencia = new object();

        /* Métodos abstractos que cada ORM concreto debe implementar */
        protected abstract string NombreArchivo { get; }
        protected abstract string NombreElemento { get; }
        protected abstract T MapearDesdeXml(XElement nodo);
        protected abstract XElement MapearAXml(T entidad);

        /* Directorio base donde se guardan los XML */
        protected virtual string DirectorioBase => XmlDataPathUtil.GetDataPath();

        /* -----------------------------------------------------------------------------------------------------
         * Función: ObtenerSiguienteId
         * Descripción: Calcula el próximo ID disponible usando max(Id) + 1.
         *              Si la lista está vacía, devuelve 1.
         -----------------------------------------------------------------------------------------------------*/
        public virtual int ObtenerSiguienteId()
        {
            List<T> lista = ObtenerTodos();
            if (lista.Count == 0)
                return 1;
            return lista.Max(e => e.Id) + 1;
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: ObtenerTodos
         * Descripción: Lee el archivo XML y devuelve todos los registros como lista de objetos.
         *              Si el archivo no existe, devuelve una lista vacía.
         -----------------------------------------------------------------------------------------------------*/
        public virtual List<T> ObtenerTodos()
        {
            lock (CandadoPersistencia)
            {
                string ruta = Path.Combine(DirectorioBase, NombreArchivo);
                if (!File.Exists(ruta))
                    return new List<T>();

                try
                {
                    XDocument doc = XDocument.Load(ruta);
                    return doc.Descendants(NombreElemento)
                              .Select(MapearDesdeXml)
                              .ToList();
                }
                catch
                {
                    return new List<T>();
                }
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: ObtenerPorId
         * Descripción: Devuelve el primer registro cuyo Id coincida con el buscado.
         *              Si no existe, devuelve null.
         -----------------------------------------------------------------------------------------------------*/
        public virtual T ObtenerPorId(int id)
        {
            return ObtenerTodos().FirstOrDefault(e => e.Id == id);
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: Insertar
         * Descripción: Asigna un nuevo Id a la entidad, la agrega a la lista y guarda el XML.
         -----------------------------------------------------------------------------------------------------*/
        public virtual void Insertar(T entidad)
        {
            lock (CandadoPersistencia)
            {
                List<T> lista = ObtenerTodos();
                entidad.Id = ObtenerSiguienteId();
                lista.Add(entidad);
                GuardarLista(lista);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: Actualizar
         * Descripción: Busca el registro por Id, lo reemplaza por el nuevo y guarda el XML.
         *              Si no lo encuentra, lanza una excepción.
         -----------------------------------------------------------------------------------------------------*/
        public virtual void Actualizar(T entidad)
        {
            lock (CandadoPersistencia)
            {
                List<T> lista = ObtenerTodos();
                int indice = lista.FindIndex(e => e.Id == entidad.Id);
                if (indice < 0)
                    throw new InvalidOperationException($"No se encontró {NombreElemento} con Id={entidad.Id}.");

                lista[indice] = entidad;
                GuardarLista(lista);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: Eliminar
         * Descripción: Elimina todos los registros cuyo Id coincida y guarda el XML.
         -----------------------------------------------------------------------------------------------------*/
        public virtual void Eliminar(int id)
        {
            lock (CandadoPersistencia)
            {
                List<T> lista = ObtenerTodos();
                lista.RemoveAll(e => e.Id == id);
                GuardarLista(lista);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: GuardarLista
         * Descripción: Convierte la lista de objetos a XML y la guarda en el archivo.
         *              El nodo raíz se llama "ArrayOf" + NombreElemento (ej: "ArrayOfCliente").
         -----------------------------------------------------------------------------------------------------*/
        protected void GuardarLista(List<T> lista)
        {
            string ruta = Path.Combine(DirectorioBase, NombreArchivo);
            string raiz = "ArrayOf" + NombreElemento;
            XDocument doc = new XDocument(
                new XElement(raiz, lista.Select(MapearAXml))
            );
            doc.Save(ruta);
        }
    }
}
