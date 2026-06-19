using DAL.ORM.BASE;
using ENTITY;
using MAPPER;
using System.Xml.Linq;

namespace DAL.ORM
{
    /* -----------------------------------------------------------------------------------------------------
     * Clase: VentaORM
     * Descripción: ORM concreto para la entidad Venta. Hereda de OrmXmlBase<Venta> y delega
     *              la conversión XML ↔ objeto al VentaMapper.
     *              Los datos se persisten en el archivo "ventas.xml".
     *              El detalle de productos de cada venta se maneja por separado en VentaProductoOrm.
     -----------------------------------------------------------------------------------------------------*/
    public class VentaORM : OrmXmlBase<Venta>
    {
        protected override string NombreArchivo => "ventas.xml";
        protected override string NombreElemento => "Venta";
        protected override Venta MapearDesdeXml(XElement nodo) => VentaMapper.Map(nodo);
        protected override XElement MapearAXml(Venta entidad) => VentaMapper.ToXml(entidad);
    }
}
