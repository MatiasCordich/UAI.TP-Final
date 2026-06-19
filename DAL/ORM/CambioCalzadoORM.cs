using DAL.ORM.BASE;
using ENTITY;
using MAPPER;
using System.Xml.Linq;

namespace DAL.ORM
{
    /* -----------------------------------------------------------------------------------------------------
     * Clase: CambioCalzadoORM
     * Descripción: ORM concreto para la entidad CambioCalzado. Hereda de OrmXmlBase<CambioCalzado>
     *              y delega la conversión XML ↔ objeto al CambioCalzadoMapper.
     *              Los datos se persisten en el archivo "cambioscalzado.xml".
     *              Registra los cambios de calzado realizados en postventa (CU03).
     -----------------------------------------------------------------------------------------------------*/
    public class CambioCalzadoORM : OrmXmlBase<CambioCalzado>
    {
        protected override string NombreArchivo => "cambioscalzado.xml";
        protected override string NombreElemento => "CambioCalzado";
        protected override CambioCalzado MapearDesdeXml(XElement nodo) => CambioCalzadoMapper.Map(nodo);
        protected override XElement MapearAXml(CambioCalzado entidad) => CambioCalzadoMapper.ToXml(entidad);
    }
}
