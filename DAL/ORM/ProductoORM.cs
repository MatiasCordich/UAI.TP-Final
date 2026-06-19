using DAL.ORM.BASE;
using ENTITY;
using MAPPER;
using System.Xml.Linq;

namespace DAL.ORM
{
    /* -----------------------------------------------------------------------------------------------------
     * Clase: ProductoORM
     * Descripción: ORM concreto para la entidad Producto. Hereda de OrmXmlBase<Producto> y delega
     *              la conversión XML ↔ objeto al ProductoMapper.
     *              Los datos se persisten en el archivo "productos.xml".
     -----------------------------------------------------------------------------------------------------*/
    public class ProductoORM : OrmXmlBase<Producto>
    {
        protected override string NombreArchivo => "productos.xml";
        protected override string NombreElemento => "Producto";
        protected override Producto MapearDesdeXml(XElement nodo) => ProductoMapper.Map(nodo);
        protected override XElement MapearAXml(Producto entidad) => ProductoMapper.ToXml(entidad);
    }
}
