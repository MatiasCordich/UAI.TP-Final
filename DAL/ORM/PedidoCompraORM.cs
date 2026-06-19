using DAL.ORM.BASE;
using ENTITY;
using MAPPER;
using System.Xml.Linq;

namespace DAL.ORM
{
    /* -----------------------------------------------------------------------------------------------------
     * Clase: PedidoCompraORM
     * Descripción: ORM concreto para la entidad PedidoCompra. Hereda de OrmXmlBase<PedidoCompra>
     *              y delega la conversión XML ↔ objeto al PedidoCompraMapper.
     *              Los datos se persisten en el archivo "pedidoscompra.xml".
     *              Registra las órdenes de compra generadas a los proveedores (CU04).
     -----------------------------------------------------------------------------------------------------*/
    public class PedidoCompraORM : OrmXmlBase<PedidoCompra>
    {
        protected override string NombreArchivo => "pedidoscompra.xml";
        protected override string NombreElemento => "PedidoCompra";
        protected override PedidoCompra MapearDesdeXml(XElement nodo) => PedidoCompraMapper.Map(nodo);
        protected override XElement MapearAXml(PedidoCompra entidad) => PedidoCompraMapper.ToXml(entidad);
    }
}
