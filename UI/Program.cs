using BLL;
using DAL.DAO;
using DAL.ORM.BASE;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            /* Verifica si hay usuarios, si no crea el usuario Dueño inicial */
            InicializarDatos();

            Application.Run(new FrmLogin());
        }



        /* -----------------------------------------------------------------------------------------------------
         * Evento: InicializarDatos
         * Descripción: Incializar con datos para crear un usuario Administrador (Dueño)
         -----------------------------------------------------------------------------------------------------*/
        private static void InicializarDatos()
        {
            try
            {
                /* Instanciar los DAO necesarios. */
                UsuarioDAO usuarioDAO = new UsuarioDAO();
                RolDAO rolDAO = new RolDAO();

                /* Si ya hay usuarios no hace nada */
                if (usuarioDAO.GetFiltered().Count > 0)
                    return;

                /* Crea los roles si no existen */
                if (rolDAO.GetAll().Count == 0)
                {
                    rolDAO.Insert(new Rol { Nombre = "Dueño" });
                    rolDAO.Insert(new Rol { Nombre = "EncargadoVentas" });
                    rolDAO.Insert(new Rol { Nombre = "EmpleadoVentas" });
                    rolDAO.Insert(new Rol { Nombre = "EncargadoCompras" });
                    rolDAO.Insert(new Rol { Nombre = "EncargadoInventario" });
                }

                /* Crea el usuario Dueño inicial */
                Usuario dueno = new Usuario
                {
                    IdRol = 1,
                    Nombre = "José",
                    Apellido = "Pérez",
                    NombreUsuario = "JPEREZ",
                    Clave = EncryptService.HashClave("Admin123!"),
                    FechaAlta = DateTime.Now
                };
                usuarioDAO.Insert(dueno);

                /* Avisa al usuario las credenciales iniciales */
                MessageBox.Show(
                    "Sistema iniciado por primera vez.\n\n" +
                    "Usuario: JPEREZADMIN\n" +
                    "Contraseña: Admin123!\n\n" +
                    "Por favor cambie la contraseña al ingresar.",
                    "Inicialización del sistema",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar el sistema: " + ex.Message,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }
    }
}
