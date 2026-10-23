using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    /*------------------------------------------------------------------------------------------------------------------
     * ENTIDAD: Rol
     * Representa los roles que va a tener cada usuario del sistema según su posición de trabajo.
     * Cada Rol apunta a un Composite raíz (IdComponente), que es el árbol de permisis del rol. 
    ------------------------------------------------------------------------------------------------------------------*/
    public class Rol : IEntidadConId
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        /* Id del Composite raíz del rol, es decir, el grupo de permisos que le corresponde. */
        public int IdComponente { get; set; }

        /* Propiedad de navegación. Es el Composite ya armado, con todo su árbol de permisos dentro. */
        public Componente Componente { get; set; }
    }
}
