using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    /*------------------------------------------------------------------------------------------------------------------
     * ENTIDAD: PedidoProducto
     * Representa representa la tabla intermedia entre PedidoCompra y Producto. 
     * Representa también cada línea del detalle de un pedido de compra. 
     * Los campos CantidadRecibida y Observacion sirven para dar una mejor trazabilidad en el caso de que haya faltante 
     * o sobrante según la cantidad recibida que indica el remito (Caso de Uso 05)
    ------------------------------------------------------------------------------------------------------------------*/
    public class PedidoProducto : IEntidadConId
    {
        public int Id { get; set; }
        public int IdPedido { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public int CantidadRecibida { get; set; }  
        public string Observacion { get; set; }     

        // Propiedad de navegacion
        public Producto Producto { get; set; }
    }
}
