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

         /* -----------------------------------------------------------------------------------------------------
         * Función: GetFiltered
         * Descripción: Obtiene TODOS los usuarios.
         *              A su vez tiene parámetros opcionales si se quiere filtrar la búsqueda. 
         * Parámetros:
         *              nombreUsuario → filtra por nombre de usuario (contiene)
         *              idRol         → filtra por rol exacto
         *              activo        → filtra por estado (true=activo, false=inactivo, null=todos)
         -----------------------------------------------------------------------------------------------------*/
        public List<Usuario> GetFiltered(string nombreUsuario = null, int? idRol = null, bool? activo = null)
        {
            try
            {
                /* Obtiene todos los usuarios del XML sin filtrar */
                List<Usuario> usuarios = orm.ObtenerTodos();
 
                /* Si se proporcionó un nombre de Usuario, se aplica el filtro. */
                if (!string.IsNullOrEmpty(nombreUsuario))
                {
                    usuarios = usuarios.Where(
                        u => u.NombreUsuario.ToUpper()
                        .Contains(nombreUsuario.ToUpper())).ToList();
                }
                    
                /* Si se propocionó algun Rol, se aplica el filtro */
                if (idRol.HasValue)
                {
                    usuarios = usuarios.Where(u => u.IdRol == idRol.Value).ToList();
                }
                    
                /* Si se proporcionó algun estado, se aplica al filtro */
                if (activo.HasValue)
                {
                    usuarios = usuarios.Where(u => u.Activo == activo.Value).ToList();
                }
                    
                /* Carga el rol de cada usuario */
                foreach (Usuario u in usuarios)
                {
                    u.Rol = rolOrm.ObtenerPorId(u.IdRol);
                }
                    
                /* Devuelve los usuarios. */
                return usuarios;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los usuarios: " + ex.Message);
            }
        }

         /* -----------------------------------------------------------------------------------------------------
         * Función: GetById
         * Descripción: Obtiene un usuario por su ID
         * Parámetros: id → id del usuario. 
         -----------------------------------------------------------------------------------------------------*/
        public Usuario GetById(int id)
        {
            try
            {
                /* Busca el usuario por ID */
                Usuario usuario = orm.ObtenerPorId(id);

                /* Si no existe, devuelve null. */
                if (usuario == null) { return null; }
                    
                /* Carga el rol del usuario */
                usuario.Rol = rolOrm.ObtenerPorId(usuario.IdRol);

                /* Devuelve el Usuario. */
                return usuario;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el usuario: " + ex.Message);
            }
        }
        /* -----------------------------------------------------------------------------------------------------
        * Función: GetByUsername
        * Descripción: Busca un usuario por su nombre de usuario. 
        * Parámetros: nombreUsuario → nombre del usuario registrado. 
        -----------------------------------------------------------------------------------------------------*/
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
                usuario.Rol = rolOrm.ObtenerPorId(usuario.IdRol);

                return usuario;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el usuario: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
        * Función: Insert
        * Descripción: Registra el usuario.
        * Parámetros: La entidad Usuario
        -----------------------------------------------------------------------------------------------------*/
        public void Insert(Usuario usuario)
        {
            try
            {
                /* Llama a la función Insertar() del ORM de Usuario. */
                orm.Insertar(usuario);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el usuario: " + ex.Message);
            }
        }


        /* -----------------------------------------------------------------------------------------------------
        * Función: Update
        * Descripción: Modifica al usuario el usuario.
        * Parámetros: La entidad Usuario
        -----------------------------------------------------------------------------------------------------*/
        public void Update(Usuario usuario)
        {
            try
            {
                /* Llama a la función Acutalizar() del ORM de Usuario. */
                orm.Actualizar(usuario);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el usuario: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
        * Función: Delete
        * Descripción: Elimina el usuario.
        * Parámetros: El id del usuario a eliminar.
        -----------------------------------------------------------------------------------------------------*/
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
