using DAL.ORM.BASE;
using ENTITY;
using MAPPER;
using System.Xml.Linq;

namespace DAL.ORM
{
    /* -----------------------------------------------------------------------------------------------------
     * Clase: UsuarioORM
     * Descripción: ORM concreto para la entidad Usuario. Hereda de OrmXmlBase<Usuario> y delega
     *              la conversión XML ↔ objeto al UsuarioMapper.
     *              Los datos se persisten en el archivo "usuarios.xml".
     *              La contraseña siempre se guarda encriptada (Desafío II).
     -----------------------------------------------------------------------------------------------------*/
    public class UsuarioORM : OrmXmlBase<Usuario>
    {
        protected override string NombreArchivo => "usuarios.xml";
        protected override string NombreElemento => "Usuario";
        protected override Usuario MapearDesdeXml(XElement nodo) => UsuarioMapper.Map(nodo);
        protected override XElement MapearAXml(Usuario entidad) => UsuarioMapper.ToXml(entidad);
    }
}
