using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    /*------------------------------------------------------------------------------------------------------------------
    * ENTIDAD: Composite
    * Es el "Composite" del patrón Composite: Representa un grupo de permisos como también de otros Composites (anidamiento). 
    * La función Contain() delega en cada hijo sin saber si es un Leaf o un Composite recorriendo todo el árbol. 
    * IdsHijos es una lista auxiliar. Ayuda a la persistencia, es decir, guarda los Id leídos del XML para que el ORM 
    * arma la lista de Hijos. 
    ------------------------------------------------------------------------------------------------------------------*/
    public class Composite : Componente
    {
        /* Lista lo que hay dentro de este Composite. Cada elemento es un Componente, es decir, 
         * puede ser un Leaf (permiso suelto) u otro Composite (grupo dentro de grupo) 
         */
        public List<Componente> Hijos { get; set; } = new List<Componente>();

        /* Lista auxiliar que SOLO usa el ORM al leer el XML.
         * En el archivo, cada grupo guarda únicamente los Id de sus hijos.
         * El ORM lee esos Id acá y después los convierte en objetos reales dentro de Hijos
         */
        public List<int> IdsHijos { get; set; } = new List<int>();

        /* Respuesta del Composite a la pregunta: ¿incluís este permiso?
         * Como es un grupo, no responde por sí mismo: le pregunta a cada uno de sus hijos. 
         * Any() devuelve true apenas UN hijo responda true. 
         * Si el hijo es un Leaf compara su nombre. 
         * Si es otro Composite, este le pregunta a los suyos hasta llegar al final del árbol. 
         * El Composite no le interesa saber que tipo de hijo es (Leaf o Composite) a todos les hace la misma pregunta. 
         */
        public override bool Contain(string nombreLeaf)
        {
            return Hijos.Any(h => h.Contain(nombreLeaf));
        }

        /* Agrega un hijo (Leaf u otro Composite) a este grupo, validando que no genere duplicados */
        public void Insert(Componente hijo)
        {

            /* No se pueden agregar nulos. */
            if (hijo  == null)
            {
                throw new Exception("El componente a agregar no puede ser nulo.");
            }

            /* Un grupo no puede meterse a sí mismo */
            if (IsSame(hijo, this)){
                throw new Exception("Un composite no puede contenerse a si mismo.");
            }

            /* 
             * Evita la referencia circular: Si el hijo es un grupo que ya contiene a este grupo en algún nivel,  
             * agregarlo haría que cada uno contenga al otro y las búsqueas no terminarían nunca. 
             */
            if (hijo is Composite groupHijo && groupHijo.Includes(this))
            {
                throw new Exception("No se puede agregar: Generaría una referencia circular.");
            }

            /* No se puede agregar dos veces el mismo componente */
            if (Hijos.Any(h => IsSame(h, hijo)))
            {
                throw new Exception("El composite ya contiene ese componente");
            }

            Hijos.Add(hijo);
        }

        /* Quita a un hijo del grupo (no lo borra del sistema, solo lo saca de esta lista.) */
        public void Delete(Componente hijo)
        {
            Hijos.RemoveAll(h => IsSame(h, hijo));
        }
        
        /* 
         * Responde si el componente buscado está en cualquier nivel.
         * Primero mira si es uno de sus hijos directos; si no, y es hijo de otro grupo, 
         * le pregunta lo mismo a ese grupo (recorre todo el árbol hacia abajo)
         */
        public bool Includes(Componente componentSearched)
        {
            return Hijos.Any(h => IsSame(h, componentSearched) || 
            (h is Composite group && group.Includes(componentSearched)));
        }

        /* Dos componentes son lo mismo si son la misma instancia o tienen el mismo Id ya persistido. */
        private static bool IsSame(Componente c1, Componente c2)
        {
            return ReferenceEquals(c1, c2) || (c1.Id != 0 && c1.Id == c2.Id);
        }
    }
}
