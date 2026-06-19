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
     * Clase: ProveedorMapper
     * Descripción: Se encarga de convertir un objeto Proveedor a un nodo XML y viceversa.
     * Map()   → lee un nodo XML y devuelve un objeto Proveedor. El CUIT llega encriptado del XML.
     * ToXml() → recibe un objeto Proveedor y lo convierte en un nodo XML para guardarlo.
     -----------------------------------------------------------------------------------------------------*/
    public class ProveedorMapper
    {
        /* Convierte un nodo XML a un objeto Proveedor */
        public static Proveedor Map(XElement nodo)
        {
            return new Proveedor
            {
                Id = (int)nodo.Element("Id"),
                Nombre = (string)nodo.Element("Nombre"),
                Cuit = (string)nodo.Element("Cuit"),      // llega encriptado
                Telefono = (string)nodo.Element("Telefono"),
                Email = (string)nodo.Element("Email"),
                Activo = (bool)nodo.Element("Activo"),
                DeudaTotal = (decimal)nodo.Element("DeudaTotal")
            };
        }

        /* Convierte un objeto Proveedor a un nodo XML */
        public static XElement ToXml(Proveedor entidad)
        {
            return new XElement("Proveedor",
                new XElement("Id", entidad.Id),
                new XElement("Nombre", entidad.Nombre),
                new XElement("Cuit", entidad.Cuit),
                new XElement("Telefono", entidad.Telefono),
                new XElement("Email", entidad.Email),
                new XElement("Activo", entidad.Activo),
                new XElement("DeudaTotal", entidad.DeudaTotal)
            );
        }
    }
}
