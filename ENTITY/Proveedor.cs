using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    /*------------------------------------------------------------------------------------------------------------------
     * ENTIDAD: Proveedor
     * Representa los datos del Proveedor de calzados. 
    ------------------------------------------------------------------------------------------------------------------*/
    public class Proveedor : IEntidadConId
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Cuit { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public bool Activo { get; set; }
        public decimal DeudaTotal { get; set; }  // Es la cuenta corriente del proveedor
    }
}
