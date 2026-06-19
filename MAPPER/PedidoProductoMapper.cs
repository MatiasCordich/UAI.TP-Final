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
     * Clase: PedidoProductoMapper
     * Descripción: Se encarga de convertir un objeto PedidoProducto a un nodo XML y viceversa.
     *              Representa el detalle de productos de un pedido de compra. Incluye la cantidad
     *              recibida y observaciones para el cotejo del remito contra el pedido (CU05).
     *              La observacion es opcional, puede no existir en el XML.
     * Map()   → lee un nodo XML y devuelve un objeto PedidoProducto.
     * ToXml() → recibe un objeto PedidoProducto y lo convierte en un nodo XML para guardarlo.
     -----------------------------------------------------------------------------------------------------*/
    public class PedidoProductoMapper
    {
        /* Convierte un nodo XML a un objeto PedidoProducto */
        public static PedidoProducto Map(XElement nodo)
        {
            /* Observacion es opcional, puede no existir en el XML */
            string observacion = (string)nodo.Element("Observacion");

            return new PedidoProducto
            {
                Id = (int)nodo.Element("Id"),
                IdPedido = (int)nodo.Element("IdPedido"),
                IdProducto = (int)nodo.Element("IdProducto"),
                Cantidad = (int)nodo.Element("Cantidad"),
                CantidadRecibida = (int)nodo.Element("CantidadRecibida"),
                Observacion = string.IsNullOrEmpty(observacion) ? null : observacion
            };
        }

        /* Convierte un objeto PedidoProducto a un nodo XML */
        public static XElement ToXml(PedidoProducto entidad)
        {
            return new XElement("PedidoProducto",
                new XElement("Id", entidad.Id),
                new XElement("IdPedido", entidad.IdPedido),
                new XElement("IdProducto", entidad.IdProducto),
                new XElement("Cantidad", entidad.Cantidad),
                new XElement("CantidadRecibida", entidad.CantidadRecibida),
                new XElement("Observacion", entidad.Observacion ?? "")
            );
        }
    }
}
