using DAL.ORM.BASE;
using ENTITY;
using MAPPER;
using System.Xml.Linq;

namespace DAL.ORM
{
    /* -----------------------------------------------------------------------------------------------------
     * Clase: ReciboORM
     * Descripción: ORM concreto para la entidad Recibo. Hereda de OrmXmlBase<Recibo> y delega
     *              la conversión XML ↔ objeto al ReciboMapper.
     *              Los datos se persisten en el archivo "recibos.xml".
     *              El recibo se genera automáticamente al confirmar un pago (CU07).
     -----------------------------------------------------------------------------------------------------*/
    public class ReciboORM : OrmXmlBase<Recibo>
    {
        protected override string NombreArchivo => "recibos.xml";
        protected override string NombreElemento => "Recibo";
        protected override Recibo MapearDesdeXml(XElement nodo) => ReciboMapper.Map(nodo);
        protected override XElement MapearAXml(Recibo entidad) => ReciboMapper.ToXml(entidad);
    }
}
