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
     * Clase: VentaMapper
     * Descripción: Se encarga de convertir un objeto Venta a un nodo XML y viceversa.
     * Map()   → lee un nodo XML y devuelve un objeto Venta con todos sus atributos.
     * ToXml() → recibe un objeto Venta y lo convierte en un nodo XML para guardarlo.
     *           No incluye el detalle de productos, ese se maneja con VentaProductoMapper.
     -----------------------------------------------------------------------------------------------------*/
    public class VentaMapper
    {
        /* Convierte un nodo XML a un objeto Venta */
        public static Venta Map(XElement nodo)
        {
            return new Venta
            {
                Id = (int)nodo.Element("Id"),
                IdCliente = (int)nodo.Element("IdCliente"),
                Fecha = (DateTime)nodo.Element("Fecha"),
                Total = (decimal)nodo.Element("Total"),
                MedioPago = (string)nodo.Element("MedioPago"),
                Descuento = (decimal)nodo.Element("Descuento"),
                BeneficioAplicado = (bool)nodo.Element("BeneficioAplicado")
            };
        }

        /* Convierte un objeto Venta a un nodo XML */
        public static XElement ToXml(Venta entidad)
        {
            return new XElement("Venta",
                new XElement("Id", entidad.Id),
                new XElement("IdCliente", entidad.IdCliente),
                new XElement("Fecha", entidad.Fecha),
                new XElement("Total", entidad.Total),
                new XElement("MedioPago", entidad.MedioPago),
                new XElement("Descuento", entidad.Descuento),
                new XElement("BeneficioAplicado", entidad.BeneficioAplicado)
            );
        }
    }
}
