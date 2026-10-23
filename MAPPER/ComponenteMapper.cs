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
     * Clase: ComponenteMapper
     * Descripción: Se encarga de convertir un objeto Componente (Leaf o Composite) en un nodo XML y viceversa.
     * Todos los componentes se guardan de forma plana, como nodos <Componente> con un campo Tipo. 
     * Los Composite no guardan copias de sus hijos, solo sus Id, así un mismo componene puede ser reutilizado
     * por varios Composite.
     * Map()   → lee un nodo XML y devuelve un Leaf o un Composite segun el Tipo. 
     * ToXml() → recibe un Leaf o un Composite y lo convierte en un nodo XML. 
     -----------------------------------------------------------------------------------------------------*/
    public class ComponenteMapper
    {
        /* Convierte un nodo XML a un Leaf o un Composite.
         * El XML guarda los dos tipos con la misma forma, así que el campo Tipo indica cuál crear. 
         */
        public static Componente Map(XElement nodo)
        {
            /* Lee del XML el texto del campo Tipo: "Leaf" o "Composite" */
            string tipo = (string)nodo.Element("Tipo");

            /* Si es un Composite (grupo), crea un Composite */
            if (tipo == "Composite")
            {
                Composite compuesto = new Composite
                {
                    Id = (int)nodo.Element("Id"),
                    Nombre = (string)nodo.Element("Nombre")
                };

                /* Busca la sección <Hijos> del nodo. Puede no existir si el grupo está vacío */
                XElement hijos = nodo.Element("Hijos");
                if (hijos != null)
                {
                    /* Lee cada <IdHijo> como número y arma la lista de Id.
                       Todavía no se crean los objetos hijos: de eso se ocupa el ORM, que tiene
                       la lista completa de componentes para buscar cada Id */
                    compuesto.IdsHijos = hijos.Elements("IdHijo")
                                              .Select(h => (int)h)
                                              .ToList();
                }

                return compuesto;
            }

            /* Si no es Composite, es un Leaf (permiso suelto): solo tiene Id y Nombre */
            return new Leaf
            {
                Id = (int)nodo.Element("Id"),
                Nombre = (string)nodo.Element("Nombre")
            };
        }

        /* Convierte un Leaf o un Composite a un nodo XML */
        public static XElement ToXml(Componente entidad)
        {
            /* Arma los datos comunes a los dos tipos: Id, Tipo y Nombre.
               El Tipo se decide mirando qué clase es el objeto: Composite o Leaf */
            XElement nodo = new XElement("Componente",
                new XElement("Id", entidad.Id),
                new XElement("Tipo", entidad is Composite ? "Composite" : "Leaf"),
                new XElement("Nombre", entidad.Nombre)
            );

            /* Si es un Composite, además guarda sus hijos.
               De cada hijo guarda solo el Id, no una copia completa del hijo */
            if (entidad is Composite compuesto)
            {
                nodo.Add(new XElement("Hijos",
                    compuesto.Hijos.Select(h => new XElement("IdHijo", h.Id))));
            }

            return nodo;
        }
    }
}

