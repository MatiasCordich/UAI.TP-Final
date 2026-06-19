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
     * Clase: PedidoCompraMapper
     * Descripción: Se encarga de convertir un objeto PedidoCompra a un nodo XML y viceversa.
     *              Representa una orden de compra a un proveedor con su estado actual.
     *              Estados posibles: Pendiente | Confirmado | Recibido | Cancelado.
     * Map()   → lee un nodo XML y devuelve un objeto PedidoCompra.
     * ToXml() → recibe un objeto PedidoCompra y lo convierte en un nodo XML para guardarlo.
     -----------------------------------------------------------------------------------------------------*/
    public class PedidoCompraMapper
    {
        /* Convierte un nodo XML a un objeto PedidoCompra */
        public static PedidoCompra Map(XElement nodo)
        {
            return new PedidoCompra
            {
                Id = (int)nodo.Element("Id"),
                IdProveedor = (int)nodo.Element("IdProveedor"),
                Fecha = (DateTime)nodo.Element("Fecha"),
                Estado = (string)nodo.Element("Estado")
            };
        }

        /* Convierte un objeto PedidoCompra a un nodo XML */
        public static XElement ToXml(PedidoCompra entidad)
        {
            return new XElement("PedidoCompra",
                new XElement("Id", entidad.Id),
                new XElement("IdProveedor", entidad.IdProveedor),
                new XElement("Fecha", entidad.Fecha),
                new XElement("Estado", entidad.Estado)
            );
        }
    }
}
