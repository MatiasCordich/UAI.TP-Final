using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    /*------------------------------------------------------------------------------------------------------------------
     * ENTIDAD: Pago
     * Representa el pago que se realiza a los proveedores. 
    ------------------------------------------------------------------------------------------------------------------*/
    public class Pago : IEntidadConId
    {
        public int Id { get; set; }
        public int IdProveedor { get; set; }
        public int IdPedido { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string MedioPago { get; set; }  // Efectivo | Transferencia | Cheque

        // Propiedades de navegacion
        public Proveedor Proveedor { get; set; }
        public PedidoCompra PedidoCompra { get; set; }
    }
}
