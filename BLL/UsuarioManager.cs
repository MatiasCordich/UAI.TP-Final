using BLL.SECURITY;
using DAL.DAO;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL
{
    /* -----------------------------------------------------------------------------------------------------
     * Clase: UsuarioManager
     * Descripción: Capa de lógica de negocio para la gestión de usuarios del sistema (Desafío I).
     *              Gestiona el alta, baja, modificación y consulta de usuarios.
     *              La contraseña siempre se hashea antes de guardar.
     *              Solo el Dueño puede gestionar usuarios.
     -----------------------------------------------------------------------------------------------------*/
    public class UsuarioManager
    {
        /* Instancia del DAO de usuarios para acceder al XML */
        private UsuarioDAO usuarioDAO = new UsuarioDAO();

        /* Instancia del servicio de autenticación para generar contraseñas automáticas */
        private AuthService authService = new AuthService();

        /* ID del rol Dueño para validar que siempre haya al menos uno activo */
        private const int IdRolDueno = 1;

        /* -----------------------------------------------------------------------------------------------------
         * Función: ListarUsuarios
         * Descripción: Obtiene todos los usuarios como también obtener una lista de usuarios filtrada. 
         * Parámetros: 
         *          - Nombre de Usuario
         *          - Rol
         *          - Estado
         *          Todos estos parámetros son opcionales. 
         * Retorna: lista de usuarios (todos o la lista filtrada según algun criterio)
         -----------------------------------------------------------------------------------------------------*/
        public List<Usuario> ListarUsuarios(string nombreUsuario = null, int? idRol = null, bool? activo = null)
        {
            try
            {
                /* Obtiene una lista completa o filtrada de los usuarios del XML. */
                return usuarioDAO.GetFiltered(nombreUsuario, idRol, activo );
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los usuarios: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: ObtenerUsuario
         * Descripción: Obtiene un usuario por su ID.
         * Parámetros: ID del usuario.
         * Retorna: usuario encontrado.
         -----------------------------------------------------------------------------------------------------*/
        public Usuario ObtenerUsuario(int id)
        {
            try
            {
                /* Busca el usuario por ID en el XML */
                Usuario usuario = usuarioDAO.GetById(id);

                /* Verifica que el usuario exista */
                if (usuario == null)
                    throw new Exception("El usuario no existe.");

                /* Devuelve el usuario encontrado */
                return usuario;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el usuario: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: RegistrarUsuario
         * Descripción: Da de alta un nuevo usuario con contraseña automática (Desafío I).
         *              La contraseña se hashea antes de guardar en el XML.
         * Parámetros: usuario con datos básicos.
         * Retorna: contraseña automática en texto plano (solo se muestra una vez al admin).
         -----------------------------------------------------------------------------------------------------*/
        public string RegistrarUsuario(Usuario usuario)
        {
            try
            {
                /* Ejecuta las validaciones de negocio centralizadas */
                ValidarUsuario(usuario);

                /* Genera el nombre de usuario automáticamente */
                usuario.NombreUsuario = GenerarNombreUsuario(usuario.Nombre, usuario.Apellido);

                /* Genera la contraseña automática que el admin le comunicará al usuario */
                string claveTemporal = authService.GenerarClaveAutomatica();

                /* Hashea la contraseña antes de guardarla en el XML */
                usuario.Clave = EncryptService.HashClave(claveTemporal);

                /* Se registra la fecha de alta */
                usuario.FechaAlta = DateTime.Now;

                /* El usuario debe cambiar la clave por primera vez. */
                usuario.DebeCambiarClave = true;

                /* Los intentos se setean a 0. */
                usuario.IntentosFallidos = 0;
            
                /* Guarda el usuario en el XML */
                usuarioDAO.Insert(usuario);

                /* Devuelve la contraseña en texto plano para que el admin la comunique al usuario */
                return claveTemporal;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el usuario: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: ModificarUsuario
         * Descripción: Modifica los datos de un usuario sin tocar la contraseña.
         * Parámetros: usuario con datos actualizados.
         -----------------------------------------------------------------------------------------------------*/
        public void ModificarUsuario(Usuario usuario)
        {
            try
            {
                /* Verifica que el usuario exista */
                Usuario existente = usuarioDAO.GetById(usuario.Id);
                if (existente == null)
                    throw new Exception("El usuario no existe.");

                /* El usuario administrador inicial no puede modificarse. */
                if (existente.Id == 1)
                    throw new Exception("El usuario administrador inicial del sistema no puede modificarse.");

                /* Verifica que el usuario esté activo */
                if (!existente.Activo)
                    throw new Exception("No se puede modificar un usuario desactivado.");

                /* Se realizan las validaciones para modificar un usuario. */
                ValidarUsuario(usuario);

                /* Verifica que no se cambie el rol si es el único Dueño activo */
                if (existente.IdRol == IdRolDueno && usuario.IdRol != IdRolDueno)
                {
                    if (EsUnicoDueno(usuario.Id))
                        throw new Exception("No se puede cambiar el rol del único Dueño activo del sistema.");
                }

                /* Actualiza los datos del usuario en el XML sin tocar la contraseña y el nombre. */
                usuario.NombreUsuario = existente.NombreUsuario;
                usuario.Clave = existente.Clave;
                usuarioDAO.Update(usuario);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar el usuario: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: DesactivarUsuario
         * Descripción: Baja lógica del usuario. No puede desactivar al último Dueño activo.
         * Parámetros: ID del usuario a desactivar e ID del usuario que ejecuta la acción.
         -----------------------------------------------------------------------------------------------------*/
        public void DesactivarUsuario(int id, int idUsuarioActual)
        {
            try
            {
                /* Verifica que el usuario a desactivar exista */
                Usuario usuario = usuarioDAO.GetById(id);
                if (usuario == null)
                    throw new Exception("El usuario no existe.");

                /* El usuario administrador inicial no puede desactivarse */
                if (id == 1)
                    throw new Exception("El usuario administrador inicial del sistema no puede desactivarse.");

                /* Verifica que el usuario a desactivar no esté ya desactivado */
                if (!usuario.Activo)
                    throw new Exception("El usuario ya está desactivado.");

                /* Verifica que no se esté desactivando a sí mismo. */
                if (id == idUsuarioActual)
                    throw new Exception("No puede desactivar su propio usuario.");

                /* Verifica que no sea el último Dueño activo del sistema */
                if (usuario.IdRol == IdRolDueno && EsUnicoDueno(id))
                    throw new Exception("No se puede desactivar al único Dueño activo del sistema.");

                /* Baja lógica: el usuario queda en el XML pero con Activo = false */
                usuario.Activo = false;
                usuarioDAO.Update(usuario);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al desactivar el usuario: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: ActivarUsuario
         * Descripción: Reactiva un usuario previamente desactivado.
         * Parámetros: ID del usuario a activar.
         -----------------------------------------------------------------------------------------------------*/
        public void ActivarUsuario(int id)
        {
            try
            {
                /* Verifica que el usuario exista */
                Usuario usuario = usuarioDAO.GetById(id);
                if (usuario == null)
                    throw new Exception("El usuario no existe.");

                /* Verifica que no esté ya activo */
                if (usuario.Activo)
                    throw new Exception("El usuario ya está activo.");

                /* Reactiva el usuario en el XML */
                usuario.Activo = true;
                usuarioDAO.Update(usuario);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al activar el usuario: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
        * Función: PreVisualizarNombreUsuario
        * Descripción: Método público para que la UI pueda previsualizar el nombre de usuario
        *              mientras el admin escribe el nombre y apellido.
        * Parámetros: nombre y apellido del usuario.
        * Retorna: nombre de usuario generado o vacío si faltan datos.
        -----------------------------------------------------------------------------------------------------*/
        public string PreVisualizarNombreUsuario(string nombre, string apellido)
        {
            /* Se validan que ambos campos (nombre y apellido) tengan valores. Caso contario devuelve un string vacío. */
            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido))
                return string.Empty;

            /* Llama al método privado de generación */
            return GenerarNombreUsuario(nombre, apellido);
        }



        /* -----------------------------------------------------------------------------------------------------
         * MÉTODOS PRIVADOS
         -----------------------------------------------------------------------------------------------------*/
        /* -----------------------------------------------------------------------------------------------------
         * Función: ValidarUsuario
         * Descripción: Centraliza todas las validaciones de negocio para alta y modificación.
         *              Se llama tanto en RegistrarUsuario como en ModificarUsuario.
         * Parámetros: usuario con datos en texto plano.
         -----------------------------------------------------------------------------------------------------*/
        private void ValidarUsuario(Usuario usuario)
        {
            /* Nombre obligatorio */
            if (string.IsNullOrEmpty(usuario.Nombre))
                throw new Exception("El nombre es obligatorio.");

            /* Apellido obligatorio */
            if (string.IsNullOrEmpty(usuario.Apellido))
                throw new Exception("El apellido es obligatorio.");

            /* Rol obligatorio */
            if (usuario.IdRol <= 0)
                throw new Exception("Debe asignar un rol al usuario.");
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: EsUnicoDueno
         * Descripción: Verifica si un usuario es el único Dueño activo del sistema.
         *              Se usa para evitar dejar el sistema sin administrador.
         * Parámetros: ID del usuario a verificar.
         * Retorna: true si es el único Dueño activo, false si hay otros.
         -----------------------------------------------------------------------------------------------------*/
        private bool EsUnicoDueno(int idUsuario)
        {
            /* Obtiene todos los usuarios que sean activos y tengan el rol de "Dueño" */
            List<Usuario> todos = usuarioDAO.GetFiltered(idRol: IdRolDueno, activo: true);

            /* Cuenta cuántos Dueños activos hay excluyendo al usuario en cuestión. */
            int cantidadDuenos = 0;
            foreach (Usuario u in todos)
            {
                if (u.IdRol == IdRolDueno && u.Activo && u.Id != idUsuario)
                    cantidadDuenos++;
            }

            /* Si no hay otros Dueños activos, este es el único */
            return cantidadDuenos == 0;
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: GenerarNombreUsuario
         * Descripción: Genera automáticamente el nombre de usuario.
         * Parámetros: nombre y apellido del usuario.
         * Retorna: nombre de usuario único generado.
         -----------------------------------------------------------------------------------------------------*/
        private string GenerarNombreUsuario(string nombre, string apellido)
        {
            /* Toma la primera letra del nombre en mayúscula */
            string primerLetraNombre = nombre.Trim().Substring(0, 1).ToUpper();

            /* Toma solo el primer apellido si hay más de uno */
            string primerApellido = apellido.Trim().Split(' ')[0].ToUpper();

            /* Combina para formar el nombre de usuario base */
            string nombreUsuarioBase = primerLetraNombre + primerApellido;

            /* Obtiene todos los usuarios para verificar duplicados */
            List<Usuario> todos = usuarioDAO.GetFiltered();

            /* Verifica si el nombre base ya existe */
            bool existe = todos.Exists(u => u.NombreUsuario == nombreUsuarioBase);

            /* Si no existe lo devuelve directamente */
            if (!existe)
                return nombreUsuarioBase;

            /* Si existe busca un número correlativo disponible */
            int numero = 1;
            string nombreUsuarioFinal;
            do
            {
                /* Formatea el número con dos dígitos (01, 02, etc.) */
                nombreUsuarioFinal = nombreUsuarioBase + numero.ToString("D2");
                numero++;
            }
            while (todos.Exists(u => u.NombreUsuario == nombreUsuarioFinal));

            /* Devuelve el nombre de usuario con el número correlativo */
            return nombreUsuarioFinal;
        }
    }
}
