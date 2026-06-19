using DAL.ORM.BASE;
using ENTITY;
using MAPPER;
using System.Xml.Linq;

namespace DAL.ORM
{
    /* -----------------------------------------------------------------------------------------------------
     * Clase: PedidoProductoORM
     * Descripción: ORM concreto para la entidad PedidoProducto. Hereda de OrmXmlBase<PedidoProducto>
     *              y delega la conversión XML ↔ objeto al PedidoProductoMapper.
     *              Los datos se persisten en el archivo "pedidoproductos.xml".
     *              Representa el detalle de productos de cada pedido de compra.
     *              Incluye la cantidad recibida y observaciones para el cotejo del remito (CU05).
     -----------------------------------------------------------------------------------------------------*/
    public class PedidoProductoORM : OrmXmlBase<PedidoProducto>
    {
        protected override string NombreArchivo => "pedidoproductos.xml";
        protected override string NombreElemento => "PedidoProducto";
        protected override PedidoProducto MapearDesdeXml(XElement nodo) => PedidoProductoMapper.Map(nodo);
        protected override XElement MapearAXml(PedidoProducto entidad) => PedidoProductoMapper.ToXml(entidad);
    }
}
