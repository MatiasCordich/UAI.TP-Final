using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    /*------------------------------------------------------------------------------------------------------------------
     * ENTIDAD: VentaProducto
     * Representa la tabla intermedia entre Venta y Producto. 
     * Representa cada línea del detalle de una venta. 
    ------------------------------------------------------------------------------------------------------------------*/
    public class VentaProducto : IEntidadConId
    {
        public int Id { get; set; }
        public int IdVenta { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }

        // Propiedades de navegacion
        public Producto Producto { get; set; }
    }
}
