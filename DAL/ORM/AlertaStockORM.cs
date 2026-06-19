using DAL.ORM.BASE;
using ENTITY;
using MAPPER;
using System.Xml.Linq;

namespace DAL.ORM
{
    /* -----------------------------------------------------------------------------------------------------
     * Clase: AlertaStockORM
     * Descripción: ORM concreto para la entidad AlertaStock. Hereda de OrmXmlBase<AlertaStock>
     *              y delega la conversión XML ↔ objeto al AlertaStockMapper.
     *              Los datos se persisten en el archivo "alertasstock.xml".
     *              Las alertas persisten hasta que el encargado las resuelva (CU10).
     -----------------------------------------------------------------------------------------------------*/
    public class AlertaStockORM : OrmXmlBase<AlertaStock>
    {
        protected override string NombreArchivo => "alertasstock.xml";
        protected override string NombreElemento => "AlertaStock";
        protected override AlertaStock MapearDesdeXml(XElement nodo) => AlertaStockMapper.Map(nodo);
        protected override XElement MapearAXml(AlertaStock entidad) => AlertaStockMapper.ToXml(entidad);
    }
}
