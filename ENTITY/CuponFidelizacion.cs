using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    /*------------------------------------------------------------------------------------------------------------------
     * ENTIDAD: CuponFidelizacion
     * Representa los cupones de fidelización para los Clientes. 
    ------------------------------------------------------------------------------------------------------------------*/
    public class CuponFidelizacion : IEntidadConId
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public string Descripcion { get; set; }
        public decimal Descuento { get; set; }      
        public DateTime FechaEmision { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public bool Enviado { get; set; }
        public bool Usado { get; set; }

        // Propiedad de navegacion
        public Cliente Cliente { get; set; }
    }
}
