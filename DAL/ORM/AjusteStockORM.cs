using DAL.ORM.BASE;
using ENTITY;
using MAPPER;
using System.Xml.Linq;

namespace DAL.ORM
{
    /* -----------------------------------------------------------------------------------------------------
     * Clase: AjusteStockORM
     * Descripción: ORM concreto para la entidad AjusteStock. Hereda de OrmXmlBase<AjusteStock>
     *              y delega la conversión XML ↔ objeto al AjusteStockMapper.
     *              Los datos se persisten en el archivo "ajustesstock.xml".
     *              Registra las bajas de stock por merma o rotura con trazabilidad (CU09).
     -----------------------------------------------------------------------------------------------------*/
    public class AjusteStockORM : OrmXmlBase<AjusteStock>
    {
        protected override string NombreArchivo => "ajustesstock.xml";
        protected override string NombreElemento => "AjusteStock";
        protected override AjusteStock MapearDesdeXml(XElement nodo) => AjusteStockMapper.Map(nodo);
        protected override XElement MapearAXml(AjusteStock entidad) => AjusteStockMapper.ToXml(entidad);
    }
}
