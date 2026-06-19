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
     * Clase: ReciboMapper
     * Descripción: Se encarga de convertir un objeto Recibo a un nodo XML y viceversa.
     *              El recibo se genera automáticamente al confirmar un pago (CU07).
     *              Queda guardado para trazabilidad y para poder imprimirlo después.
     * Map()   → lee un nodo XML y devuelve un objeto Recibo.
     * ToXml() → recibe un objeto Recibo y lo convierte en un nodo XML para guardarlo.
     -----------------------------------------------------------------------------------------------------*/
    public class ReciboMapper
    {
        /* Convierte un nodo XML a un objeto Recibo */
        public static Recibo Map(XElement nodo)
        {
            return new Recibo
            {
                Id = (int)nodo.Element("Id"),
                IdPago = (int)nodo.Element("IdPago"),
                FechaEmision = (DateTime)nodo.Element("FechaEmision"),
                NumeroRecibo = (string)nodo.Element("NumeroRecibo")
            };
        }

        /* Convierte un objeto Recibo a un nodo XML */
        public static XElement ToXml(Recibo entidad)
        {
            return new XElement("Recibo",
                new XElement("Id", entidad.Id),
                new XElement("IdPago", entidad.IdPago),
                new XElement("FechaEmision", entidad.FechaEmision),
                new XElement("NumeroRecibo", entidad.NumeroRecibo)
            );
        }
    }
}
