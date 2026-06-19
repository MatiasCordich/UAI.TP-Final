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
     * Clase: PagoMapper
     * Descripción: Se encarga de convertir un objeto Pago a un nodo XML y viceversa.
     *              Representa un pago realizado a un proveedor asociado a un pedido de compra.
     *              El monto y medio de pago se consideran datos sensibles (Desafío II).
     * Map()   → lee un nodo XML y devuelve un objeto Pago. Monto y MedioPago llegan encriptados.
     * ToXml() → recibe un objeto Pago y lo convierte en un nodo XML para guardarlo.
     -----------------------------------------------------------------------------------------------------*/
    public class PagoMapper
    {
        /* Convierte un nodo XML a un objeto Pago */
        public static Pago Map(XElement nodo)
        {
            return new Pago
            {
                Id = (int)nodo.Element("Id"),
                IdProveedor = (int)nodo.Element("IdProveedor"),
                IdPedido = (int)nodo.Element("IdPedido"),
                Fecha = (DateTime)nodo.Element("Fecha"),
                Monto = (decimal)nodo.Element("Monto"),      // llega encriptado
                MedioPago = (string)nodo.Element("MedioPago")    // llega encriptado
            };
        }

        /* Convierte un objeto Pago a un nodo XML */
        public static XElement ToXml(Pago entidad)
        {
            return new XElement("Pago",
                new XElement("Id", entidad.Id),
                new XElement("IdProveedor", entidad.IdProveedor),
                new XElement("IdPedido", entidad.IdPedido),
                new XElement("Fecha", entidad.Fecha),
                new XElement("Monto", entidad.Monto),
                new XElement("MedioPago", entidad.MedioPago)
            );
        }
    }
}
