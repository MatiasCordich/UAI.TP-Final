using DAL.ORM;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;

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
        /* Instancia del ORM de usuarios para acceder al XML */
        private UsuarioORM orm = new UsuarioORM();

        /* Instancia del ORM de roles para cargar el rol del usuario */
        private RolORM rolOrm = new RolORM();

        public List<Usuario> GetAll()
        {
            try
            {
                /* Obtiene todos los usuarios activos del XML */
                List<Usuario> usuarios = orm.ObtenerTodos().Where(u => u.Activo).ToList();

                /* Carga el rol de cada usuario */
                foreach (Usuario u in usuarios)
                {
                    u.Rol = rolOrm.ObtenerPorId(u.IdRol);
                }

                /* Devuelve la lista de usuarios*/
                return usuarios;
                    
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
                /* Busca el usuario por ID */
                Usuario usuario = orm.ObtenerPorId(id);

                /* Si no existe, devuelve null. */
                if (usuario == null)
                    return null;

                /* Carga el rol del usuario */
                usuario.Rol = rolOrm.ObtenerPorId(usuario.IdRol);

                return usuario;
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
                /* Busca el usuario por nombre de usuario en el XML */
                Usuario usuario = orm.ObtenerTodos()
                             .FirstOrDefault(u => u.NombreUsuario == nombreUsuario && u.Activo);

                /* Si no existe, devuelve null. */
                if (usuario == null)
                    return null;

                /* Carga el rol del usuario */
                RolORM rolOrm = new RolORM();
                usuario.Rol = rolOrm.ObtenerPorId(usuario.IdRol);

                return usuario;
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
