using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    /*------------------------------------------------------------------------------------------------------------------
     * ENTIDAD: Rol
     * Reprsenta los roles que va a tener cada usuario del sistema según su posición de trabajo.
    ------------------------------------------------------------------------------------------------------------------*/
    public class Rol : IEntidadConId
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}
