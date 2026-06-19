using DAL.ORM.BASE;
using ENTITY;
using MAPPER;
using System.Xml.Linq;

namespace DAL.ORM
{
    /* -----------------------------------------------------------------------------------------------------
     * Clase: ProveedorORM
     * Descripción: ORM concreto para la entidad Proveedor. Hereda de OrmXmlBase<Proveedor> y delega
     *              la conversión XML ↔ objeto al ProveedorMapper.
     *              Los datos se persisten en el archivo "proveedores.xml".
     *              El CUIT se guarda encriptado (Desafío II).
     -----------------------------------------------------------------------------------------------------*/
    public class ProveedorORM : OrmXmlBase<Proveedor>
    {
        protected override string NombreArchivo => "proveedores.xml";
        protected override string NombreElemento => "Proveedor";
        protected override Proveedor MapearDesdeXml(XElement nodo) => ProveedorMapper.Map(nodo);
        protected override XElement MapearAXml(Proveedor entidad) => ProveedorMapper.ToXml(entidad);
    }
}
