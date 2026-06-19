using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    /* -----------------------------------------------------------------------------------------------------
     * Clase: EncryptService
     * Descripción: Servicio de encriptación y desencriptación de datos sensibles (Desafío II).
     *              Utiliza AES (Advanced Encryption Standard) que es un algoritmo simétrico seguro.
     *              Los datos sensibles del sistema son:
     *              - Contraseñas de usuarios
     *              - DNI de clientes
     *              - Email de clientes
     *              - CUIT de proveedores
     *              - Monto y medio de pago
     *              Los datos siempre se guardan encriptados en el XML y solo se
     *              desencriptan cuando se muestran en la interfaz gráfica.
     -----------------------------------------------------------------------------------------------------*/
    public class EncryptService
    {
        /* Clave de 32 bytes (256 bits) para AES-256 */
        private static readonly string Clave = "BabiloniaCalzados2026!@#$%^&*()A";

        /* Vector de inicialización de 16 bytes */
        private static readonly string VectorInicializacion = "BabiloniaIV2026!";

        /* -----------------------------------------------------------------------------------------------------
         * Función: Encriptar
         * Descripción: Encripta un texto plano usando AES-256 y lo devuelve en Base64.
         * Parámetros: texto plano a encriptar.
         * Retorna: texto encriptado en Base64.
         -----------------------------------------------------------------------------------------------------*/
        public static string Encriptar(string textoplano)
        {
            try
            {
                /* Se valida si hay texto para encriptar. */
                if (string.IsNullOrEmpty(textoplano))
                    return textoplano;
                
                /* Convierte la clave y el vector en bytes. */
                byte[] claveBytes = Encoding.UTF8.GetBytes(Clave);
                byte[] vectorBytes = Encoding.UTF8.GetBytes(VectorInicializacion);

                using (Aes aes = Aes.Create())
                {
                    /* Se asigna la clave y el vector al objeto AES */
                    aes.Key = claveBytes;
                    aes.IV = vectorBytes;

                    /* Se crea el encriptador */
                    using (ICryptoTransform encriptador = aes.CreateEncryptor())
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            /* Se usa CryptoStream para aplicar la encriptación mientras escribe */
                            using (CryptoStream cs = new CryptoStream(ms, encriptador, CryptoStreamMode.Write))
                            {
                                byte[] textoBytes = Encoding.UTF8.GetBytes(textoplano);
                                cs.Write(textoBytes, 0, textoBytes.Length);
                                cs.FlushFinalBlock();
                            }

                            /* Se deuvelve el resultado converitdo a Base64 para guardarlo en el XML */
                            return Convert.ToBase64String(ms.ToArray());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al encriptar el dato: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: Desencriptar
         * Descripción: Desencripta un texto en Base64 usando AES-256 y lo devuelve en texto plano.
         * Parámetros: texto encriptado en Base64.
         * Retorna: texto plano desencriptado.
         -----------------------------------------------------------------------------------------------------*/
        public static string Desencriptar(string textoEncriptado)
        {
            try
            {

                /* Si el texto está vacío no hay nada para encirptar. */
                if (string.IsNullOrEmpty(textoEncriptado))
                    return textoEncriptado;

                /* Convierte la clave y el vector en bytes. */
                byte[] claveBytes = Encoding.UTF8.GetBytes(Clave);
                byte[] vectorBytes = Encoding.UTF8.GetBytes(VectorInicializacion);

                /* Convierte el texto Base64 de vuelta a bytes */
                byte[] textoBytes = Convert.FromBase64String(textoEncriptado);

                using (Aes aes = Aes.Create())
                {
                    /* Se le asigna la clave y el vector al objeto AES */
                    aes.Key = claveBytes;
                    aes.IV = vectorBytes;

                    /* Se crea el desincriptador. */
                    using (ICryptoTransform desencriptador = aes.CreateDecryptor())
                    {
                        using (MemoryStream ms = new MemoryStream(textoBytes))
                        {
                            /* Se utliza CryptoStream ya que aplica la desencriptación mientras lee. */
                            using (CryptoStream cs = new CryptoStream(ms, desencriptador, CryptoStreamMode.Read))
                            {
                                /* Se lee el texto desencriptado */
                                using (StreamReader sr = new StreamReader(cs))
                                {
                                    /* Se devuelve el texto desencriptado. */
                                    return sr.ReadToEnd();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al desencriptar el dato: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: HashContrasena
         * Descripción: Genera un hash SHA-256 de una contraseña. Se usa para almacenar
         *              contraseñas de forma segura. El hash es unidireccional, no se puede
         *              desencriptar, solo se puede comparar.
         * Parámetros: contraseña en texto plano.
         * Retorna: hash SHA-256 en Base64.
         -----------------------------------------------------------------------------------------------------*/
        public static string HashClave(string contrasena)
        {
            try
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    /* Convierte la contraseña a bytes y calcula el hash */
                    byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(contrasena));

                    /* Convierte el hash a Base64 para guardarlo */
                    return Convert.ToBase64String(bytes);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al hashear la contraseña: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: VerificarContrasena
         * Descripción: Compara una contraseña en texto plano con su hash almacenado.
         * Parámetros: contraseña en texto plano y hash almacenado.
         * Retorna: true si coinciden, false si no.
         -----------------------------------------------------------------------------------------------------*/
        public static bool VerificarClave(string contrasena, string hashAlmacenado)
        {
            try
            {
                /* Se hashea la contraseña ingresada y la compara con la almacenada */
                string hashNuevo = HashClave(contrasena);

                /* Devuelve un booleano como resultado de la comparación de contraseñas. */
                return hashNuevo == hashAlmacenado;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar la contraseña: " + ex.Message);
            }
        }
    }
}
