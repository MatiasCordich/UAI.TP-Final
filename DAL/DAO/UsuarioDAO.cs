using DAL.ORM;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAO
{
    /* -----------------------------------------------------------------------------------------------------
     * Clase: UsuarioDAO
     * Descripción: Capa de acceso a datos para la entidad Usuario.
     *              Utiliza UsuarioORM para realizar las operaciones de persistencia en el archivo XML.
     *              Incluye métodos específicos para el login y gestión de usuarios (Desafío I).
     -----------------------------------------------------------------------------------------------------*/
    public class UsuarioDAO
    {
        private UsuarioORM orm = new UsuarioORM();

        public List<Usuario> GetAll()
        {
            try
            {
                return orm.ObtenerTodos();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los usuarios: " + ex.Message);
            }
        }

        public Usuario GetById(int id)
        {
            try
            {
                return orm.ObtenerPorId(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el usuario: " + ex.Message);
            }
        }

        /* Busca un usuario por su nombre de usuario - usado en el login (Desafío I) */
        public Usuario GetByUsername(string nombreUsuario)
        {
            try
            {
                return orm.ObtenerTodos()
                          .FirstOrDefault(u => u.NombreUsuario == nombreUsuario && u.Activo);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el usuario: " + ex.Message);
            }
        }

        public void Insert(Usuario usuario)
        {
            try
            {
                orm.Insertar(usuario);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el usuario: " + ex.Message);
            }
        }

        public void Update(Usuario usuario)
        {
            try
            {
                orm.Actualizar(usuario);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el usuario: " + ex.Message);
            }
        }

        public void Delete(int id)
        {
            try
            {
                orm.Eliminar(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el usuario: " + ex.Message);
            }
        }
    }
}
