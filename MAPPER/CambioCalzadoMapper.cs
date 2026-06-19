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
     * Clase: CambioCalzadoMapper
     * Descripción: Se encarga de convertir un objeto CambioCalzado a un nodo XML y viceversa.
     *              Registra el cambio de calzado en postventa: qué producto devolvió el cliente
     *              y cuál se llevó, vinculado a la venta original (CU03).
     * Map()   → lee un nodo XML y devuelve un objeto CambioCalzado.
     * ToXml() → recibe un objeto CambioCalzado y lo convierte en un nodo XML para guardarlo.
     -----------------------------------------------------------------------------------------------------*/
    public class CambioCalzadoMapper
    {
        /* Convierte un nodo XML a un objeto CambioCalzado */
        public static CambioCalzado Map(XElement nodo)
        {
            return new CambioCalzado
            {
                Id = (int)nodo.Element("Id"),
                IdVentaOriginal = (int)nodo.Element("IdVentaOriginal"),
                IdProductoViejo = (int)nodo.Element("IdProductoViejo"),
                IdProductoNuevo = (int)nodo.Element("IdProductoNuevo"),
                Fecha = (DateTime)nodo.Element("Fecha")
            };
        }

        /* Convierte un objeto CambioCalzado a un nodo XML */
        public static XElement ToXml(CambioCalzado entidad)
        {
            return new XElement("CambioCalzado",
                new XElement("Id", entidad.Id),
                new XElement("IdVentaOriginal", entidad.IdVentaOriginal),
                new XElement("IdProductoViejo", entidad.IdProductoViejo),
                new XElement("IdProductoNuevo", entidad.IdProductoNuevo),
                new XElement("Fecha", entidad.Fecha)
            );
        }
    }
}
