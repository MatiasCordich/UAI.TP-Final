using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    /*------------------------------------------------------------------------------------------------------------------
     * ENTIDAD: Producto
     * Representa los datos del producto.
    ------------------------------------------------------------------------------------------------------------------*/
    public class Producto : IEntidadConId
    {
        public int Id { get; set; }
        public int IdProveedor { get; set; }
        public string Modelo { get; set; }
        public int Talle { get; set; }
        public string Color { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int StockMinimo { get; set; } 
        public bool Activo { get; set; }

        // Propiedad de navegacion
        public Proveedor Proveedor { get; set; }
    }
}
