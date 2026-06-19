using DAL.ORM.BASE;
using ENTITY;
using MAPPER;
using System.Xml.Linq;

namespace DAL.ORM
{
    /* -----------------------------------------------------------------------------------------------------
     * Clase: RolORM
     * Descripción: ORM concreto para la entidad Rol. Hereda de OrmXmlBase<Rol> y delega
     *              la conversión XML ↔ objeto al RolMapper.
     *              Los datos se persisten en el archivo "roles.xml".
     -----------------------------------------------------------------------------------------------------*/
    public class RolORM : OrmXmlBase<Rol>
    {
        protected override string NombreArchivo => "roles.xml";
        protected override string NombreElemento => "Rol";
        protected override Rol MapearDesdeXml(XElement nodo) => RolMapper.Map(nodo);
        protected override XElement MapearAXml(Rol entidad) => RolMapper.ToXml(entidad);
    }
}
