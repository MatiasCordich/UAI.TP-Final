using DAL.ORM.BASE;
using ENTITY;
using MAPPER;
using System.Xml.Linq;

namespace DAL.ORM
{
    /* -----------------------------------------------------------------------------------------------------
     * Clase: PagoORM
     * Descripción: ORM concreto para la entidad Pago. Hereda de OrmXmlBase<Pago> y delega
     *              la conversión XML ↔ objeto al PagoMapper.
     *              Los datos se persisten en el archivo "pagos.xml".
     *              Registra los pagos realizados a los proveedores (CU06).
     *              Monto y MedioPago se guardan encriptados (Desafío II).
     -----------------------------------------------------------------------------------------------------*/
    public class PagoORM : OrmXmlBase<Pago>
    {
        protected override string NombreArchivo => "pagos.xml";
        protected override string NombreElemento => "Pago";
        protected override Pago MapearDesdeXml(XElement nodo) => PagoMapper.Map(nodo);
        protected override XElement MapearAXml(Pago entidad) => PagoMapper.ToXml(entidad);
    }
}
