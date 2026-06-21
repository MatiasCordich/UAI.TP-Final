using DAL.DAO;
using ENTITY;
using System;
using System.Text.RegularExpressions;

namespace BLL.SECURITY
{
    /* -----------------------------------------------------------------------------------------------------
     * Clase: AuthService
     * Descripción: Servicio de autenticación y gestión del ciclo de vida de usuarios (Desafío I).
     *              Cubre:
     *              - Login/Logout con identificación y autenticación
     *              - Generación automática de contraseña al crear usuario
     *              - Cambio obligatorio en el primer ingreso
     *              - Validación de contraseña segura
     *              - Bloqueo por intentos fallidos
     *              - Desbloqueo de usuario
     *              - Vencimiento de contraseña por plazo configurable
     *              - Restauración de contraseña por palabra clave
     -----------------------------------------------------------------------------------------------------*/
    public class AuthService
    {
        /* Se instancia el DAO de usuarios. */
        private UsuarioDAO usuarioDAO = new UsuarioDAO();

        /* Intentos máximos antes de bloquear (parametrizable). */
        private const int MaxIntentosFallidos = 3;

        /* Días de vigencia de la contraseña (parametrizable). */
        private const int DiasVigenciaContrasena = 90;

        /* Código especial para indicar que el usuario debe cambiar su clave */
        public const string CodigoDebeCambiarClave = "DEBE_CAMBIAR_CLAVE";

        /* ID del usuario admin inicial que nunca se bloquea */
        private const int IdUsuarioAdmin = 1;

        /* Usuario actualmente logueado en el sistema */
        public static Usuario UsuarioActual { get; private set; }

        /* -----------------------------------------------------------------------------------------------------
         * Función: Login
         * Descripción: Identifica y autentica al usuario en el sistema.
         *              Verifica que el usuario exista, no esté bloqueado y la contraseña sea correcta.
         *              Si debe cambiar la clave lanza un código especial para que la UI lo detecte.
         * Parámetros: nombre de usuario y contraseña en texto plano.
         * Retorna: el usuario autenticado.
         -----------------------------------------------------------------------------------------------------*/
        public Usuario Login(string nombreUsuario, string contrasena)
        {
            try
            {
                /* Se busca el usuario por nombre de usuario en el XML */
                Usuario usuario = usuarioDAO.GetByUsername(nombreUsuario);

                /* Se verifica que el usuario exista */
                if (usuario == null)
                    throw new Exception("El usuario no existe en el sistema.");

                /* Se verifica que el usuario esté activo (desactivado o bloqueado por intentos) */
                if (!usuario.Activo)
                    throw new Exception("El usuario está desactivado o bloqueado. Contacte al administrador.");

                /* Se verifica la contraseña comparando el hash */
                if (!EncryptService.VerificarClave(contrasena, usuario.Clave))
                {
                    /* Si la contraseña es incorrecta, se registra el intento fallido */
                    RegistrarIntentoFallido(usuario);
                    throw new Exception("Contraseña incorrecta.");
                }

                /* Se resetean los intentos fallidos al ingresar correctamente */
                ResetearIntentosFallidos(usuario);

                /* Se verifica si la contraseña está vencida (excepto el admin inicial) */
                if (usuario.Id != IdUsuarioAdmin && ClaveVencida(usuario))
                    throw new Exception(CodigoDebeCambiarClave);

                /* Se verifica si debe cambiar la clave en este ingreso (excepto el admin inicial) */
                if (usuario.Id != IdUsuarioAdmin && usuario.DebeCambiarClave)
                    throw new Exception(CodigoDebeCambiarClave);

                /* Se guarda el usuario actual en sesión */
                UsuarioActual = usuario;

                /* Se retorna el usuario autenticado. */
                return usuario;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

         /* -----------------------------------------------------------------------------------------------------
         * Función: LoginParcial
         * Descripción: Autentica al usuario pero sin guardarlo en sesión.
         *              Se usa cuando el usuario debe cambiar la clave antes de ingresar.
         *              Devuelve el usuario para que formulario de cambiar clave pueda operar.
         * Parámetros: nombre de usuario y contraseña en texto plano.
         * Retorna: el usuario autenticado sin guardarlo en sesión.
         -----------------------------------------------------------------------------------------------------*/
        public Usuario LoginParcial(string nombreUsuario, string contrasena)
        {
            try
            {
                /* Busca el usuario por nombre de usuario */
                Usuario usuario = usuarioDAO.GetByUsername(nombreUsuario);
 
                /* Verifica que el usuario exista. */
                if (usuario == null)
                    throw new Exception("El usuario no existe en el sistema.");
 
                /* Valida que la contraseña sea la correcta. */
                if (!EncryptService.VerificarClave(contrasena, usuario.Clave))
                    throw new Exception("Contraseña incorrecta.");
 
                /* Devuelve el usuario sin guardarlo en sesión */
                return usuario;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en la autenticación: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: GuardarSesion
         * Descripción: Guarda el usuario en sesión después de cambiar la contraseña.
         -----------------------------------------------------------------------------------------------------*/
        public void GuardarSesion(Usuario usuario)
        {
            UsuarioActual = usuario;
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: Logout
         * Descripción: Cierra la sesión del usuario actual.
         -----------------------------------------------------------------------------------------------------*/
        public void Logout()
        {
            /* Se setea el usuario actual a null. */
            UsuarioActual = null;
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: GenerarClaveAutomatica
         * Descripción: Genera una contraseña aleatoria segura para el alta de un nuevo usuario.
         * Retorna: Contraseña en texto plano (solo se muestra una vez al administrador).
         -----------------------------------------------------------------------------------------------------*/
        public string GenerarClaveAutomatica()
        {
            try
            {
                /* Se definen los caracteres permitidos. */
                const string mayusculas = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                const string minusculas = "abcdefghijklmnopqrstuvwxyz";
                const string numeros = "0123456789";
                const string simbolos = "!@#$%";

                /* Se instancia el objeto para randomizar la contraseña. */
                Random rnd = new Random();
                string contrasena = "";

                /* Se garantiza al menos un carácter de cada tipo */
                contrasena += mayusculas[rnd.Next(mayusculas.Length)];
                contrasena += minusculas[rnd.Next(minusculas.Length)];
                contrasena += numeros[rnd.Next(numeros.Length)];
                contrasena += simbolos[rnd.Next(simbolos.Length)];

                /* Se completa la contraseña hasta 10 caracteres */
                string todos = mayusculas + minusculas + numeros + simbolos;
                for (int i = 4; i < 10; i++)
                    contrasena += todos[rnd.Next(todos.Length)];

                /* Se mezclan los caracteres para que no sigan un patrón predecible. */
                char[] array = contrasena.ToCharArray();
                for (int i = array.Length - 1; i > 0; i--)
                {
                    int j = rnd.Next(i + 1);
                    char temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                }

                /* Se vuelve transformar el array de caracteres a la contraseña en texto plano y se retorna. */
                return new string(array);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar la contraseña: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: ValidarClaveSegura
         * Descripción: Verifica que la contraseña cumpla con los requisitos de seguridad.
         * Parámetros: contraseña en texto plano y datos del usuario para evitar datos personales.
         * Retorna: true si es segura, false si no.
         -----------------------------------------------------------------------------------------------------*/
        public bool ValidarClaveSegura(string contrasena, Usuario usuario)
        {
            /* Mínimo 8 caracteres */
            if (contrasena.Length < 8)
                return false;

            /* Al menos una mayúscula */
            if (!Regex.IsMatch(contrasena, "[A-Z]"))
                return false;

            /* Al menos una minúscula */
            if (!Regex.IsMatch(contrasena, "[a-z]"))
                return false;

            /* Al menos un número */
            if (!Regex.IsMatch(contrasena, "[0-9]"))
                return false;

            /* Al menos un símbolo */
            if (!Regex.IsMatch(contrasena, "[!@#$%^&*()]"))
                return false;

            /* No puede contener el apellido del usuario */
            if (!string.IsNullOrEmpty(usuario.Apellido) &&
                contrasena.ToLower().Contains(usuario.Apellido.ToLower()))
                return false;

            /* No puede contener el nombre de usuario */
            if (!string.IsNullOrEmpty(usuario.NombreUsuario) &&
                contrasena.ToLower().Contains(usuario.NombreUsuario.ToLower()))
                return false;

            return true;
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: CambiarClave
         * Descripción: Cambia la contraseña del usuario validando que sea segura.
         *              Al cambiarla exitosamente marca DebeCambiarClave = false.
         * Parámetros: usuario, contraseña actual y nueva contraseña.
         -----------------------------------------------------------------------------------------------------*/
        public void CambiarClave(Usuario usuario, string contrasenaActual, string contrasenaNueva)
        {
            try
            {
                /* Verifica que la contraseña actual sea correcta */
                if (!EncryptService.VerificarClave(contrasenaActual, usuario.Clave))
                    throw new Exception("La contraseña actual es incorrecta.");

                /* Valida que la nueva contraseña cumpla con los requisitos de seguridad */
                if (!ValidarClaveSegura(contrasenaNueva, usuario))
                    throw new Exception("La nueva contraseña no cumple con los requisitos de seguridad.");

                /* Verifica que la nueva contraseña sea diferente a la anterior */
                if (EncryptService.VerificarClave(contrasenaNueva, usuario.Clave))
                    throw new Exception("La nueva contraseña no puede ser igual a la anterior.");

                /* Guarda el hash de la nueva contraseña */
                usuario.Clave = EncryptService.HashClave(contrasenaNueva);

                /* Marca que ya no debe cambiar la clave */
                usuario.DebeCambiarClave = false;

                /* Guarda los cambios en el XML */
                usuarioDAO.Update(usuario);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cambiar la contraseña: " + ex.Message);
            }
        }

         /* -----------------------------------------------------------------------------------------------------
         * Función: ResetearClave
         * Descripción: El Administrador resetea la clave de un usuario generando una nueva automática.
         *              El usuario deberá cambiarla en su próximo ingreso.
         * Parámetros: ID del usuario al que se le resetea la clave.
         * Retorna: nueva clave temporal en texto plano para que el admin la comunique.
         -----------------------------------------------------------------------------------------------------*/
        public string ResetearClave(int idUsuario)
        {
            try
            {
                /* Verifica que el usuario exista */
                Usuario usuario = usuarioDAO.GetById(idUsuario);
                if (usuario == null)
                    throw new Exception("El usuario no existe.");
 
                /* Genera la nueva clave automática */
                string nuevaClaveTemporal = GenerarClaveAutomatica();
 
                /* Hashea y guarda la nueva clave */
                usuario.Clave = EncryptService.HashClave(nuevaClaveTemporal);
 
                /* Obliga al usuario a cambiar la clave en el próximo ingreso */
                usuario.DebeCambiarClave = true;
 
                /* Guarda los cambios en el XML */
                usuarioDAO.Update(usuario);
 
                /* Devuelve la clave temporal para que el Administardor la comunique */
                return nuevaClaveTemporal;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al resetear la contraseña: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: ClaveVencida
         * Descripción: Verifica si la contraseña del usuario ha vencido.
         -----------------------------------------------------------------------------------------------------*/
        private bool ClaveVencida(Usuario usuario)
        {
            return (DateTime.Today - usuario.FechaAlta).Days > DiasVigenciaContrasena;
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: RegistrarIntentoFallido
         * Descripción: Registra un intento fallido de login.
         *              Si supera el máximo, bloquea al usuario.
         *              El usuario admin inicial (Id = 1) nunca se bloquea.
         -----------------------------------------------------------------------------------------------------*/
        private void RegistrarIntentoFallido(Usuario usuario)
        {
            try
            {
                /* El usuario admin inicial nunca se bloquea */
                if (usuario.Id == IdUsuarioAdmin)
                    return;

                /* Se incrementa el contador de intentos fallidos */
                usuario.IntentosFallidos++;

                /* Si supera el máximo de intentos se bloquea el usuario */
                if (usuario.IntentosFallidos >= MaxIntentosFallidos)
                {
                    /* Se desactiva el usuario */
                    usuario.Activo = false;

                    /* Se resetea el contador para cuando sea desbloqueado */
                    usuario.IntentosFallidos = 0;
                }

                /* Se usa el DAO para guardar los cambios. */
                usuarioDAO.Update(usuario);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar intento fallido: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: ResetearIntentosFallidos
         * Descripción: Resetea el contador de intentos fallidos al ingresar correctamente.
         -----------------------------------------------------------------------------------------------------*/
        private void ResetearIntentosFallidos(Usuario usuario)
        {
            try
            {
                /* Se resetea el contador de intentos fallidos */
                usuario.IntentosFallidos = 0;

                /* Se usa el DAO de usuarios para guardar los cambios. */
                usuarioDAO.Update(usuario);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al resetear intentos fallidos: " + ex.Message);
            }
        }
    }
}
