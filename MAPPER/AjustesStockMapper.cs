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
     * Clase: AjusteStockMapper
     * Descripción: Se encarga de convertir un objeto AjusteStock a un nodo XML y viceversa.
     *              Registra las bajas de stock por merma o rotura con trazabilidad completa (CU09).
     *              La cantidad siempre es negativa ya que representa una baja de stock.
     * Map()   → lee un nodo XML y devuelve un objeto AjusteStock.
     * ToXml() → recibe un objeto AjusteStock y lo convierte en un nodo XML para guardarlo.
     -----------------------------------------------------------------------------------------------------*/
    public class AjusteStockMapper
    {
        /* Convierte un nodo XML a un objeto AjusteStock */
        public static AjusteStock Map(XElement nodo)
        {
            return new AjusteStock
            {
                Id = (int)nodo.Element("Id"),
                IdProducto = (int)nodo.Element("IdProducto"),
                Cantidad = (int)nodo.Element("Cantidad"),
                Motivo = (string)nodo.Element("Motivo"),
                Fecha = (DateTime)nodo.Element("Fecha")
            };
        }

        /* Convierte un objeto AjusteStock a un nodo XML */
        public static XElement ToXml(AjusteStock entidad)
        {
            return new XElement("AjusteStock",
                new XElement("Id", entidad.Id),
                new XElement("IdProducto", entidad.IdProducto),
                new XElement("Cantidad", entidad.Cantidad),
                new XElement("Motivo", entidad.Motivo),
                new XElement("Fecha", entidad.Fecha)
            );
        }
    }
}
