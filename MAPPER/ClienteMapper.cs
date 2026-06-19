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
     * Clase: ClienteMapper
     * Descripción: Se encarga de convertir un objeto Cliente a un nodo XML y viceversa.
     * Map()   → lee un nodo XML y devuelve un objeto Cliente. FechaNacimiento es nullable,
     *           puede no existir en el XML. Nombre, DNI y Email llegan encriptados.
     * ToXml() → recibe un objeto Cliente y lo convierte en un nodo XML para guardarlo.
     -----------------------------------------------------------------------------------------------------*/
    public class ClienteMapper
    {
        /* Convierte un nodo XML a un objeto Cliente */
        public static Cliente Map(XElement nodo)
        {
            /* FechaNacimiento es nullable, puede no existir en el XML */
            string fechaNac = (string)nodo.Element("FechaNacimiento");

            return new Cliente
            {
                Id = (int)nodo.Element("Id"),
                Nombre = (string)nodo.Element("Nombre"),       // llega encriptado
                Apellido = (string)nodo.Element("Apellido"),
                Dni = (string)nodo.Element("Dni"),          // llega encriptado
                Telefono = (string)nodo.Element("Telefono"),
                Email = (string)nodo.Element("Email"),        // llega encriptado
                FechaNacimiento = string.IsNullOrEmpty(fechaNac) ? (DateTime?)null : DateTime.Parse(fechaNac),
                EsJubilado = (bool)nodo.Element("EsJubilado"),
                Activo = (bool)nodo.Element("Activo"),
                FechaAlta = (DateTime)nodo.Element("FechaAlta")
            };
        }

        /* Convierte un objeto Cliente a un nodo XML */
        public static XElement ToXml(Cliente entidad)
        {
            return new XElement("Cliente",
                new XElement("Id", entidad.Id),
                new XElement("Nombre", entidad.Nombre),
                new XElement("Apellido", entidad.Apellido),
                new XElement("Dni", entidad.Dni),
                new XElement("Telefono", entidad.Telefono),
                new XElement("Email", entidad.Email),
                new XElement("FechaNacimiento", entidad.FechaNacimiento?.ToString("yyyy-MM-dd") ?? ""),
                new XElement("EsJubilado", entidad.EsJubilado),
                new XElement("Activo", entidad.Activo),
                new XElement("FechaAlta", entidad.FechaAlta)
            );
        }
    }
}
