using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    /*------------------------------------------------------------------------------------------------------------------
     * ENTIDAD: Componente
     * Es el "Component" del patrón Composite: la clase abstracta común que está por encima de Leaf y Composite.
     * 
     * En el sistema representa un permiso, ya sea individual (Leaf) o un grupo de permisos (Composite),
     * de modo que el resto del código pueda tratarlos exactamente igual.
     * 
     * Contain() responde si el componente (o alguno de sus hijos) incluye un Leaf determinado (se lo busca por su nombre).
     *
     * Para entenderlo fácil: es como "algo que está en una carpeta de la computadora".
     * Ese algo puede ser un archivo (Leaf) o una carpeta (Composite), pero de los dos se puede preguntar lo mismo.
    ------------------------------------------------------------------------------------------------------------------*/

    public abstract class Componente : IEntidadConId
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        /* Indica si este componente (o alguno de sus hijos) inlcuye el permiso buscado. */
        /* Cada hijo (Leaf y Composite) está OBLIGADO a responder por eso heredan esta función. */
        public abstract bool Contain(string nombreLeaf);
    }
}
