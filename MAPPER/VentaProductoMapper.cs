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
     * Clase: VentaProductoMapper
     * Descripción: Se encarga de convertir un objeto VentaProducto a un nodo XML y viceversa.
     *              Representa cada línea de detalle de una venta (qué producto y en qué cantidad).
     * Map()   → lee un nodo XML y devuelve un objeto VentaProducto.
     * ToXml() → recibe un objeto VentaProducto y lo convierte en un nodo XML para guardarlo.
     -----------------------------------------------------------------------------------------------------*/
    public class VentaProductoMapper
    {
        /* Convierte un nodo XML a un objeto VentaProducto */
        public static VentaProducto Map(XElement nodo)
        {
            return new VentaProducto
            {
                Id = (int)nodo.Element("Id"),
                IdVenta = (int)nodo.Element("IdVenta"),
                IdProducto = (int)nodo.Element("IdProducto"),
                Cantidad = (int)nodo.Element("Cantidad")
            };
        }

        /* Convierte un objeto VentaProducto a un nodo XML */
        public static XElement ToXml(VentaProducto entidad)
        {
            return new XElement("VentaProducto",
                new XElement("Id", entidad.Id),
                new XElement("IdVenta", entidad.IdVenta),
                new XElement("IdProducto", entidad.IdProducto),
                new XElement("Cantidad", entidad.Cantidad)
            );
        }
    }
}
