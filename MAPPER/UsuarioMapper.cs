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
     * Clase: UsuarioMapper
     * Descripción: Se encarga de convertir un objeto Usuario a un nodo XML y viceversa.
     * Map()   → lee un nodo XML y devuelve un objeto Usuario. La contraseña llega encriptada del XML.
     * ToXml() → recibe un objeto Usuario y lo convierte en un nodo XML. La contraseña se guarda encriptada.
     -----------------------------------------------------------------------------------------------------*/
    public class UsuarioMapper
    {
        /* Convierte un nodo XML a un objeto Usuario */
        public static Usuario Map(XElement nodo)
        {
            return new Usuario
            {
                Id = (int)nodo.Element("Id"),
                IdRol = (int)nodo.Element("IdRol"),
                Nombre = (string)nodo.Element("Nombre"),
                Apellido = (string)nodo.Element("Apellido"),
                NombreUsuario = (string)nodo.Element("NombreUsuario"),
                Clave = (string)nodo.Element("Clave"),  // llega encriptada del XML
                Activo = (bool)nodo.Element("Activo"),
                FechaAlta = (DateTime)nodo.Element("FechaAlta")
            };
        }

        /* Convierte un objeto Usuario a un nodo XML */
        public static XElement ToXml(Usuario entidad)
        {
            return new XElement("Usuario",
                new XElement("Id", entidad.Id),
                new XElement("IdRol", entidad.IdRol),
                new XElement("Nombre", entidad.Nombre),
                new XElement("Apellido", entidad.Apellido),
                new XElement("NombreUsuario", entidad.NombreUsuario),
                new XElement("Clave", entidad.Clave),  // se guarda encriptada
                new XElement("Activo", entidad.Activo),
                new XElement("FechaAlta", entidad.FechaAlta)
            );
        }
    }
}
