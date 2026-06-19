using DAL.ORM.BASE;
using ENTITY;
using MAPPER;
using System.Xml.Linq;

namespace DAL.ORM
{
    /* -----------------------------------------------------------------------------------------------------
     * Clase: ConciliacionDiariaORM
     * Descripción: ORM concreto para la entidad ConciliacionDiaria. Hereda de 
     *              OrmXmlBase<ConciliacionDiaria> y delega la conversión XML ↔ objeto 
     *              al ConciliacionDiariaMapper.
     *              Los datos se persisten en el archivo "conciliaciones.xml".
     *              Se genera al cierre de jornada para verificar pagos vs egresos (CU08).
     -----------------------------------------------------------------------------------------------------*/
    public class ConciliacionDiariaORM : OrmXmlBase<ConciliacionDiaria>
    {
        protected override string NombreArchivo => "conciliaciones.xml";
        protected override string NombreElemento => "ConciliacionDiaria";
        protected override ConciliacionDiaria MapearDesdeXml(XElement nodo) => ConciliacionDiariaMapper.Map(nodo);
        protected override XElement MapearAXml(ConciliacionDiaria entidad) => ConciliacionDiariaMapper.ToXml(entidad);
    }
}
