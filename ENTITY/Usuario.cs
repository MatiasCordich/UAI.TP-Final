using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    /*------------------------------------------------------------------------------------------------------------------
     * ENTIDAD: Usuario
     * Representa a los usuarios del sistema que tendrán un rol en específico según su posición. 
    ------------------------------------------------------------------------------------------------------------------*/
    public class Usuario : IEntidadConId
    {
        public int Id { get; set; }
        public int IdRol { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NombreUsuario { get; set; }
        public string Clave { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaAlta { get; set; }

        // Propiedad de navegacion
        public Rol Rol { get; set; }
    }
}
