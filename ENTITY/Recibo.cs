using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    /*------------------------------------------------------------------------------------------------------------------
     * ENTIDAD: Recibo
     * Representa la generación y registro automático del Recibo una vez que se haya confirmado el pago (Caso de Uso 07).
    ------------------------------------------------------------------------------------------------------------------*/
    public class Recibo : IEntidadConId
    {
        public int Id { get; set; }
        public int IdPago { get; set; }
        public DateTime FechaEmision { get; set; }
        public string NumeroRecibo { get; set; } 

        // Propiedad de navegacion
        public Pago Pago { get; set; }

    }
}
