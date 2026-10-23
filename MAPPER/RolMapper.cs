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
     * Clase: RolMapper
     * Descripción: Se encarga de convertir un objeto Rol a un nodo XML y viceversa.
     * Map()   → lee un nodo XML y devuelve un objeto Rol.
     * ToXml() → recibe un objeto Rol y lo convierte en un nodo XML para guardarlo.
     -----------------------------------------------------------------------------------------------------*/

    public class RolMapper
    {
        /* Convierte un nodo XML a un objeto Rol */
        public static Rol Map(XElement nodo)
        {
            return new Rol
            {
                Id = (int)nodo.Element("Id"),
                Nombre = (string)nodo.Element("Nombre"),

                /* (int?) permite que el campo no exista en el XML sin dar error.
                   "?? 0" significa: si no existe, usar 0 (rol sin permisos asignados todavía).
                   Así el roles.xml viejo, que no tiene este campo, sigue leyéndose bien */
                IdComponente = (int?)nodo.Element("IdComponente") ?? 0
            };
        }

        /* Convierte un objeto Rol a un nodo XML */
        public static XElement ToXml(Rol entidad)
        {
            return new XElement("Rol",
                new XElement("Id", entidad.Id),
                new XElement("Nombre", entidad.Nombre),

                /* NUEVO. Guarda el Id del Composite raíz del rol */
                new XElement("IdComponente", entidad.IdComponente)
            );
        }
    }

}

