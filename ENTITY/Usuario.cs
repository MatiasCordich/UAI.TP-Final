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
        public bool Activo { get; set; } = true;
        public DateTime FechaAlta { get; set; }

        /* true cuando el usuario debe cambiar su clave en el próximo ingreso */
        /* El admin (Id = 1) siempre tiene este campo en false */
        public bool DebeCambiarClave { get; set; }

        /* Contador de intentos fallidos de login */
        /* Al llegar al máximo el usuario se bloquea */
        public int IntentosFallidos { get; set; }

        // Propiedad de navegacion
        public Rol Rol { get; set; }
    }
}
