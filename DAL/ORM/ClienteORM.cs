using DAL.ORM.BASE;
using ENTITY;
using MAPPER;
using System.Xml.Linq;

namespace DAL.ORM
{
    /* -----------------------------------------------------------------------------------------------------
     * Clase: ClienteORM
     * Descripción: ORM concreto para la entidad Cliente. Hereda de OrmXmlBase<Cliente> y delega
     *              la conversión XML ↔ objeto al ClienteMapper.
     *              Los datos se persisten en el archivo "clientes.xml".
     *              Nombre, DNI y Email se guardan encriptados (Desafío II).
     -----------------------------------------------------------------------------------------------------*/
    public class ClienteORM : OrmXmlBase<Cliente>
    {
        protected override string NombreArchivo => "clientes.xml";
        protected override string NombreElemento => "Cliente";
        protected override Cliente MapearDesdeXml(XElement nodo) => ClienteMapper.Map(nodo);
        protected override XElement MapearAXml(Cliente entidad) => ClienteMapper.ToXml(entidad);
    }
}
