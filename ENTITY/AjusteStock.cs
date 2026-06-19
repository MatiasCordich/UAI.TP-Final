using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    /*------------------------------------------------------------------------------------------------------------------
     * ENTIDAD: AjusteStock
     * Representa el ajuste del stock de calzados. Sirve para  dar una mejor trazabilidad de la merma del stock 
     * debido a las roturas de los calzados (Caso de Uso 09)
    ------------------------------------------------------------------------------------------------------------------*/
    public class AjusteStock : IEntidadConId
    {
        public int Id { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }   
        public string Motivo { get; set; }
        public DateTime Fecha { get; set; }

        // Propiedad de navegacion
        public Producto Producto { get; set; }
    }
}
