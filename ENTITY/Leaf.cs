using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    /*------------------------------------------------------------------------------------------------------------------
    * ENTIDAD: Leaf
    * Es el "Leaf" (hoja) del patrón Composite: Representa un permiso individual e indivisible, es decir, la autorización
    * concreta para realizar una acción concreta del sistema (ej: "Nueva venta"). 
    * No tiene hijos, por eso la función Contain() solo compara su propio nombre. 
    ------------------------------------------------------------------------------------------------------------------*/
    public class Leaf : Componente
    {
        /* Como un Leaf no tiene nada dentro suyo sólo puede ser él mismo. */
        public override bool Contain(string nombreLeaf)
        {
            /* Responde True si su Nombre es justo el permiso que se busca y false si no. */
           return Nombre == nombreLeaf;
        }
    }
}
