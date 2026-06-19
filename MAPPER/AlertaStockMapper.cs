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
     * Clase: AlertaStockMapper
     * Descripción: Se encarga de convertir un objeto AlertaStock a un nodo XML y viceversa.
     *              Las alertas se generan cuando el stock llega al mínimo configurado (CU10).
     *              Persisten en el XML hasta que el encargado las resuelva (Resuelta = false).
     * Map()   → lee un nodo XML y devuelve un objeto AlertaStock.
     * ToXml() → recibe un objeto AlertaStock y lo convierte en un nodo XML para guardarlo.
     -----------------------------------------------------------------------------------------------------*/
    public class AlertaStockMapper
    {
        /* Convierte un nodo XML a un objeto AlertaStock */
        public static AlertaStock Map(XElement nodo)
        {
            return new AlertaStock
            {
                Id = (int)nodo.Element("Id"),
                IdProducto = (int)nodo.Element("IdProducto"),
                Fecha = (DateTime)nodo.Element("Fecha"),
                Resuelta = (bool)nodo.Element("Resuelta")
            };
        }

        /* Convierte un objeto AlertaStock a un nodo XML */
        public static XElement ToXml(AlertaStock entidad)
        {
            return new XElement("AlertaStock",
                new XElement("Id", entidad.Id),
                new XElement("IdProducto", entidad.IdProducto),
                new XElement("Fecha", entidad.Fecha),
                new XElement("Resuelta", entidad.Resuelta)
            );
        }
    }
}
