using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    /*------------------------------------------------------------------------------------------------------------------
     * ENTIDAD: ConciliaciónDiaria
     * Representa el reporte diario que se debe generar y registrar al cierre de la jornada (Caso de uso 08)
    ------------------------------------------------------------------------------------------------------------------*/
    public class ConciliacionDiaria : IEntidadConId
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal TotalPagos { get; set; }
        public decimal TotalEgresos { get; set; }
        public decimal Diferencia => TotalPagos - TotalEgresos;  // Se calcula automáticamente
        public bool TieneAlerta { get; set; }
        public string Observacion { get; set; }
    }
}
