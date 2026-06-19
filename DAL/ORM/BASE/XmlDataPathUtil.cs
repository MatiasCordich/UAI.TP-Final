using System;
using System.IO;


namespace DAL.ORM.BASE
{
    /* -----------------------------------------------------------------------------------------------------
     * Clase: XmlDataPathUtil
     * Descripción: Centraliza la gestión de rutas de los archivos XML del sistema.
     *              Todos los archivos de datos se guardan en la carpeta "datos" dentro
     *              del directorio de ejecución de la aplicación.
     *              Si la carpeta no existe, la crea automáticamente.
     -----------------------------------------------------------------------------------------------------*/
    public class XmlDataPathUtil
    {
        private static string rutaDatos;
        private static string rutaAuditoria;

        /* GetDataPath() : Devuelve la ruta donde se guardan los XML de datos. */
        public static string GetDataPath()
        {
            if (rutaDatos == null)
                rutaDatos = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "datos");

            Directory.CreateDirectory(rutaDatos);
            return rutaDatos;
        }

        /*  GetAuditoriaPath() : Devuelve la ruta donde se guardan los XML de auditoría. */
        public static string GetAuditoriaPath()
        {
            if (rutaAuditoria == null)
                rutaAuditoria = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "auditoria");

            Directory.CreateDirectory(rutaAuditoria);
            return rutaAuditoria;
        }

        /*  SetDataPath() : Permite cambiar la ruta de datos (útil para testing).*/
        public static void SetDataPath(string ruta)
        {
            rutaDatos = ruta;
            Directory.CreateDirectory(rutaDatos);
        }
    }
}
