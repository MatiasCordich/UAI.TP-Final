using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    /*------------------------------------------------------------------------------------------------------------------
     * ENTIDAD: Venta
     * Representa la venta del calzado. 
    ------------------------------------------------------------------------------------------------------------------*/
    public class Venta : IEntidadConId
    {
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string MedioPago { get; set; }  // Efectivo | Debito | Credito | Transferencia
        public decimal Descuento { get; set; }  // Porcentaje aplicado para jubilados
        public bool BeneficioAplicado { get; set; }

        // Propiedad de navegacion
        public Cliente Cliente { get; set; }
        public List<VentaProducto> Productos { get; set; }
    }
}
