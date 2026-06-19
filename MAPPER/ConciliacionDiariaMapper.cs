using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MAPPER
{
    /* -----------------------------------------------------------------------------------------------------
     * Clase: ConciliacionDiariaMapper
     * Descripción: Se encarga de convertir un objeto ConciliacionDiaria a un nodo XML y viceversa.
     *              Se genera al cierre de jornada para verificar que los pagos registrados coincidan
     *              con los egresos reales de caja/banco (CU08). La Observacion es opcional.
     *              La propiedad Diferencia NO se persiste en el XML ya que es calculada automáticamente
     *              como TotalPagos - TotalEgresos.
     * Map()   → lee un nodo XML y devuelve un objeto ConciliacionDiaria.
     * ToXml() → recibe un objeto ConciliacionDiaria y lo convierte en un nodo XML para guardarlo.
     -----------------------------------------------------------------------------------------------------*/
    public class ConciliacionDiariaMapper
    {
        /* Convierte un nodo XML a un objeto ConciliacionDiaria */
        public static ConciliacionDiaria Map(XElement nodo)
        {
            /* Observacion es opcional, puede no existir en el XML */
            string observacion = (string)nodo.Element("Observacion");

            return new ConciliacionDiaria
            {
                Id = (int)nodo.Element("Id"),
                Fecha = (DateTime)nodo.Element("Fecha"),
                TotalPagos = (decimal)nodo.Element("TotalPagos"),
                TotalEgresos = (decimal)nodo.Element("TotalEgresos"),
                TieneAlerta = (bool)nodo.Element("TieneAlerta"),
                Observacion = string.IsNullOrEmpty(observacion) ? null : observacion
                /* Diferencia no se guarda en XML, se calcula automaticamente */
            };
        }

        /* Convierte un objeto ConciliacionDiaria a un nodo XML */
        public static XElement ToXml(ConciliacionDiaria entidad)
        {
            return new XElement("ConciliacionDiaria",
                new XElement("Id", entidad.Id),
                new XElement("Fecha", entidad.Fecha),
                new XElement("TotalPagos", entidad.TotalPagos),
                new XElement("TotalEgresos", entidad.TotalEgresos),
                new XElement("TieneAlerta", entidad.TieneAlerta),
                new XElement("Observacion", entidad.Observacion ?? "")
            /* Diferencia no se persiste, es una propiedad calculada */
            );
        }
    }
}
