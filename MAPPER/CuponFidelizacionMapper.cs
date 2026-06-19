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
     * Clase: CuponFidelizacionMapper
     * Descripción: Se encarga de convertir un objeto CuponFidelizacion a un nodo XML y viceversa.
     *              Los cupones se generan por cumpleaños o por historial de compras del cliente (CU11).
     *              Tienen vigencia limitada y se marcan como enviados y usados.
     * Map()   → lee un nodo XML y devuelve un objeto CuponFidelizacion.
     * ToXml() → recibe un objeto CuponFidelizacion y lo convierte en un nodo XML para guardarlo.
     -----------------------------------------------------------------------------------------------------*/
    public class CuponFidelizacionMapper
    {
        /* Convierte un nodo XML a un objeto CuponFidelizacion */
        public static CuponFidelizacion Map(XElement nodo)
        {
            return new CuponFidelizacion
            {
                Id = (int)nodo.Element("Id"),
                IdCliente = (int)nodo.Element("IdCliente"),
                Descripcion = (string)nodo.Element("Descripcion"),
                Descuento = (decimal)nodo.Element("Descuento"),
                FechaEmision = (DateTime)nodo.Element("FechaEmision"),
                FechaVencimiento = (DateTime)nodo.Element("FechaVencimiento"),
                Enviado = (bool)nodo.Element("Enviado"),
                Usado = (bool)nodo.Element("Usado")
            };
        }

        /* Convierte un objeto CuponFidelizacion a un nodo XML */
        public static XElement ToXml(CuponFidelizacion entidad)
        {
            return new XElement("CuponFidelizacion",
                new XElement("Id", entidad.Id),
                new XElement("IdCliente", entidad.IdCliente),
                new XElement("Descripcion", entidad.Descripcion),
                new XElement("Descuento", entidad.Descuento),
                new XElement("FechaEmision", entidad.FechaEmision),
                new XElement("FechaVencimiento", entidad.FechaVencimiento),
                new XElement("Enviado", entidad.Enviado),
                new XElement("Usado", entidad.Usado)
            );
        }
    }
}
