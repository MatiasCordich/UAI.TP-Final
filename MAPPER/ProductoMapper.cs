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
     * Clase: ProductoMapper
     * Descripción: Se encarga de convertir un objeto Producto a un nodo XML y viceversa.
     * Map()   → lee un nodo XML y devuelve un objeto Producto con todos sus atributos.
     * ToXml() → recibe un objeto Producto y lo convierte en un nodo XML para guardarlo.
     -----------------------------------------------------------------------------------------------------*/
    public class ProductoMapper
    {
        /* Convierte un nodo XML a un objeto Producto */
        public static Producto Map(XElement nodo)
        {
            return new Producto
            {
                Id = (int)nodo.Element("Id"),
                IdProveedor = (int)nodo.Element("IdProveedor"),
                Modelo = (string)nodo.Element("Modelo"),
                Talle = (int)nodo.Element("Talle"),
                Color = (string)nodo.Element("Color"),
                Precio = (decimal)nodo.Element("Precio"),
                Stock = (int)nodo.Element("Stock"),
                StockMinimo = (int)nodo.Element("StockMinimo"),
                Activo = (bool)nodo.Element("Activo")
            };
        }

        /* Convierte un objeto Producto a un nodo XML */
        public static XElement ToXml(Producto entidad)
        {
            return new XElement("Producto",
                new XElement("Id", entidad.Id),
                new XElement("IdProveedor", entidad.IdProveedor),
                new XElement("Modelo", entidad.Modelo),
                new XElement("Talle", entidad.Talle),
                new XElement("Color", entidad.Color),
                new XElement("Precio", entidad.Precio),
                new XElement("Stock", entidad.Stock),
                new XElement("StockMinimo", entidad.StockMinimo),
                new XElement("Activo", entidad.Activo)
            );
        }
    }

}
