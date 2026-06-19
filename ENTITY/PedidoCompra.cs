using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    /*------------------------------------------------------------------------------------------------------------------
     * ENTIDAD: PedidoCompra
     * Representa el pedido que sea realizó a un proveedor y el estado del mismo (Pendiente | Confirmado | Recibido | Cancelado) 
    ------------------------------------------------------------------------------------------------------------------*/
    public class PedidoCompra : IEntidadConId
    {
        public int Id { get; set; }
        public int IdProveedor { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; } 

        // Propiedades de navegacion
        public Proveedor Proveedor { get; set; }
        public List<PedidoProducto> Productos { get; set; }
    }
}
