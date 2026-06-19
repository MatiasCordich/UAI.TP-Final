using DAL.ORM.BASE;
using ENTITY;
using MAPPER;
using System.Xml.Linq;

namespace DAL.ORM
{
    /* -----------------------------------------------------------------------------------------------------
     * Clase: VentaProductoORM
     * Descripción: ORM concreto para la entidad VentaProducto. Hereda de OrmXmlBase<VentaProducto>
     *              y delega la conversión XML ↔ objeto al VentaProductoMapper.
     *              Los datos se persisten en el archivo "ventaproductos.xml".
     *              Representa el detalle de productos de cada venta (qué producto y en qué cantidad).
     -----------------------------------------------------------------------------------------------------*/
    public class VentaProductoORM : OrmXmlBase<VentaProducto>
    {
        protected override string NombreArchivo => "ventaproductos.xml";
        protected override string NombreElemento => "VentaProducto";
        protected override VentaProducto MapearDesdeXml(XElement nodo) => VentaProductoMapper.Map(nodo);
        protected override XElement MapearAXml(VentaProducto entidad) => VentaProductoMapper.ToXml(entidad);
    }
}
