using DAL.ORM.BASE;
using ENTITY;
using MAPPER;
using System.Xml.Linq;

namespace DAL.ORM
{
    /* -----------------------------------------------------------------------------------------------------
     * Clase: CuponFidelizacionORM
     * Descripción: ORM concreto para la entidad CuponFidelizacion. Hereda de 
     *              OrmXmlBase<CuponFidelizacion> y delega la conversión XML ↔ objeto 
     *              al CuponFidelizacionMapper.
     *              Los datos se persisten en el archivo "cupones.xml".
     *              Los cupones se generan por cumpleaños o historial de compras (CU11).
     -----------------------------------------------------------------------------------------------------*/
    public class CuponFidelizacionORM : OrmXmlBase<CuponFidelizacion>
    {
        protected override string NombreArchivo => "cupones.xml";
        protected override string NombreElemento => "CuponFidelizacion";
        protected override CuponFidelizacion MapearDesdeXml(XElement nodo) => CuponFidelizacionMapper.Map(nodo);
        protected override XElement MapearAXml(CuponFidelizacion entidad) => CuponFidelizacionMapper.ToXml(entidad);
    }
}
