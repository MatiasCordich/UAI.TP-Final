using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    /*------------------------------------------------------------------------------------------------------------------
     * ENTIDAD: CambioCalzado
     * Represneta el registro del cambio de calzado en postventa (Caso de Uso 03). Guardando qué producto se devolvió 
     * y cuál se entregó a cambio. 
    ------------------------------------------------------------------------------------------------------------------*/
    public class CambioCalzado : IEntidadConId
    {
        public int Id { get; set; }
        public int IdVentaOriginal { get; set; }
        public int IdProductoViejo { get; set; }  // El que devuelve el cliente
        public int IdProductoNuevo { get; set; }  // El que se lleva el cliente
        public DateTime Fecha { get; set; }

        // Propiedades de navegacion
        public Venta VentaOriginal { get; set; }
        public Producto ProductoViejo { get; set; }
        public Producto ProductoNuevo { get; set; }
    }
}
