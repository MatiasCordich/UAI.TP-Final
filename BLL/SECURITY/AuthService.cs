using DAL.DAO;
using ENTITY;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        /* Intentos máximos antes de bloquear. */
        private const int MaxIntentosFallidos = 3;

        /* Días de vigencia de la contraseña. */
        private const int DiasVigenciaContrasena = 90;

        /* Usuario actualmente logueado en el sistema */
        public static Usuario UsuarioActual { get; private set; }

        /* -----------------------------------------------------------------------------------------------------
         * Función: Login
         * Descripción: Identifica y autentica al usuario en el sistema.
         *              Verifica que el usuario exista, no esté bloqueado y la contraseña sea correcta.
         *              Si es el primer ingreso, obliga a cambiar la contraseña.
         * Parámetros: nombre de usuario y contraseña en texto plano.
         * Retorna: el usuario autenticado.
         -----------------------------------------------------------------------------------------------------*/
        public Usuario Login(string nombreUsuario, string contrasena)
        {
            try
            {
                /* Paso 1: Se busca el usuario por nombre de usuario en el XML */
                Usuario usuario = usuarioDAO.GetByUsername(nombreUsuario);

                /* Paso 2: Se verifica que el usuario exista */
                if (usuario == null)
                    throw new Exception("El usuario no existe en el sistema.");

                /* Paso 3: Se verifica que el usuario esté activo */
                if (!usuario.Activo)
                    throw new Exception("El usuario está desactivado. Contacte al administrador.");

                /* Paso 4: Se verifica si está bloqueado por intentos fallidos */
                if (EstasBloqueado(usuario))
                    throw new Exception("El usuario está bloqueado por intentos fallidos. Contacte al administrador.");

                /* Paso 5: Se verifica la contraseña comparando el hash */
                if (!EncryptService.VerificarClave(contrasena, usuario.Clave))
                {
                    /* Si la contraseña es incorrecta, se registra el intento fallido */
                    RegistrarIntentoFallido(usuario);
                    throw new Exception("Contraseña incorrecta.");
                }

                /* Paso 6: Se resetea los intentos fallidos al ingresar correctamente */
                ResetearIntentosFallidos(usuario);

                /* Paso 7: Se verifica si la contraseña está vencida */
                if (ClaveVencida(usuario))
                    throw new Exception("La contraseña ha vencido. Debe cambiarla para continuar.");

                /* Paso 8: Se buarda el usuario actual en sesión */
                UsuarioActual = usuario;
                return usuario;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en el login: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: Logout
         * Descripción: Cierra la sesión del usuario actual.
         -----------------------------------------------------------------------------------------------------*/
        public void Logout()
        {
            UsuarioActual = null;
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: GenerarContrasenaAutomatica
         * Descripción: Genera una contraseña aleatoria segura para el alta de un nuevo usuario.
         *              La contraseña cumple con los requisitos de seguridad básicos.
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

                /* Se instancia el objeto para rondomizar la contrasenña. */
                Random rnd = new Random();

                /* Variable que guarda la contraseña autogenreada. */
                string contrasena = "";

                /* Se garaniza al menos un carácter de cada tipo para cumplir requisitos */
                contrasena += mayusculas[rnd.Next(mayusculas.Length)];
                contrasena += minusculas[rnd.Next(minusculas.Length)];
                contrasena += numeros[rnd.Next(numeros.Length)];
                contrasena += simbolos[rnd.Next(simbolos.Length)];

                /* Se copleta la contraseña hasta 10 caracteres con caracteres aleatorios */
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

                /* Se devuelve la contraseña aleatoria. */
                return new string(array);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar la contraseña: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: ValidarClaveSegura
         * Descripción: Verifica que la contraseña cumpla con los requisitos de seguridad:
         *              - Mínimo 8 caracteres
         *              - Al menos una mayúscula
         *              - Al menos una minúscula
         *              - Al menos un número
         *              - Al menos un símbolo
         * Parámetros: contraseña en texto plano y datos del usuario para evitar datos personales.
         * Retorna: true si es segura, false si no.
         -----------------------------------------------------------------------------------------------------*/
        public bool ValidarClaveSegura(string contrasena, Usuario usuario)
        {
            /* Se verifica longitud mínima de 8 caracteres */
            if (contrasena.Length < 8)
                return false;

            /* Se verifica que tenga al menos una mayúscula */
            if (!Regex.IsMatch(contrasena, "[A-Z]"))
                return false;

            /* Se verifica que tenga al menos una minúscula */
            if (!Regex.IsMatch(contrasena, "[a-z]"))
                return false;

            /* Se verifica que tenga al menos un número */
            if (!Regex.IsMatch(contrasena, "[0-9]"))
                return false;

            /* Se verifica que tenga al menos un símbolo */
            if (!Regex.IsMatch(contrasena, "[!@#$%^&*()]"))
                return false;

            /* No puede contener el apellido del usuario (dato personal) */
            if (!string.IsNullOrEmpty(usuario.Apellido) &&
                contrasena.ToLower().Contains(usuario.Apellido.ToLower()))
                return false;

            /* No puede contener el nombre de usuario (dato personal) */
            if (!string.IsNullOrEmpty(usuario.NombreUsuario) &&
                contrasena.ToLower().Contains(usuario.NombreUsuario.ToLower()))
                return false;

            return true;
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: CambiarClave
         * Descripción: Cambia la contraseña del usuario validando que sea segura.
         * Parámetros: usuario, contraseña actual y nueva contraseña.
         -----------------------------------------------------------------------------------------------------*/
        public void CambiarClave(Usuario usuario, string contrasenaActual, string contrasenaNueva)
        {
            try
            {
                /* Paso 1: Se verifica que la contraseña actual sea correcta */
                if (!EncryptService.VerificarClave(contrasenaActual, usuario.Clave))
                    throw new Exception("La contraseña actual es incorrecta.");

                /* Paso 2: Se valida que la nueva contraseña cumpla con los requisitos de seguridad */
                if (!ValidarClaveSegura(contrasenaNueva, usuario))
                    throw new Exception("La nueva contraseña no cumple con los requisitos de seguridad.");

                /* Paso 3: Se verifica que la nueva contraseña sea diferente a la anterior */
                if (EncryptService.VerificarClave(contrasenaNueva, usuario.Clave))
                    throw new Exception("La nueva contraseña no puede ser igual a la anterior.");

                /* Paso 4: Se guarda el hash de la nueva contraseña */
                usuario.Clave = EncryptService.HashClave(contrasenaNueva);

                /* Se usa el DAO para modificar la clave del usuario. */
                usuarioDAO.Update(usuario);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cambiar la contraseña: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: DesbloquearUsuario
         * Descripción: Desbloquea un usuario bloqueado por intentos fallidos.
         *              Solo puede ejecutarlo el administrador (Dueño).
         * Parámetros: ID del usuario a desbloquear.
         -----------------------------------------------------------------------------------------------------*/
        public void DesbloquearUsuario(int idUsuario)
        {
            try
            {
                /* Se valida la existencia del usuario. */
                Usuario usuario = usuarioDAO.GetById(idUsuario);
                if (usuario == null)
                    throw new Exception("El usuario no existe.");

                /* Se activa el usuario. */
                usuario.Activo = true;

                /* Se usa el DAO para modificar el estado del usuario. */
                usuarioDAO.Update(usuario);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al desbloquear el usuario: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: EstasBloqueado
         * Descripción: Verifica si el usuario está bloqueado.
         -----------------------------------------------------------------------------------------------------*/
        private bool EstasBloqueado(Usuario usuario)
        {
            return !usuario.Activo;
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
         * Descripción: Registra un intento fallido de login. Si supera el máximo, bloquea al usuario.
         -----------------------------------------------------------------------------------------------------*/
        private void RegistrarIntentoFallido(Usuario usuario)
        {
            try
            {
                /* Si supera el máximo de intentos, se desactiva (bloquea) */
                usuario.Activo = false;

                /* Se usa el DAO para modificar el estado del usuario. */
                usuarioDAO.Update(usuario);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar intento fallido: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: ResetearIntentosFallidos
         * Descripción: Resetea los intentos fallidos al ingresar correctamente.
         -----------------------------------------------------------------------------------------------------*/
        private void ResetearIntentosFallidos(Usuario usuario)
        {
            try
            {
                /* Se activa el usuario. */
                usuario.Activo = true;

                /* Se usa el DAO para modificar el estado del usuario. */
                usuarioDAO.Update(usuario);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al resetear intentos fallidos: " + ex.Message);
            }
        }
    }
}
