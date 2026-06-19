using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    /*------------------------------------------------------------------------------------------------------------------
     * ENTIDAD: AlertaStock
     * Representa una notifiacion al encargado cuando un calzado crítico (que tiene mucho movimiento) se encuentra cerca
     * del stock mínimo. Esta alerta va persistir hasta que el encargado lo resuelva (Caso de Uso 10)
    ------------------------------------------------------------------------------------------------------------------*/
    public class AlertaStock : IEntidadConId
    {
        public int Id { get; set; }
        public int IdProducto { get; set; }
        public DateTime Fecha { get; set; }
        public bool Resuelta { get; set; }  

        // Propiedad de navegacion
        public Producto Producto { get; set; }
    }
}
