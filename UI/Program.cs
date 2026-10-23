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

                /* Manager que crea el árbol de permisos (patrón Composite) y se lo asigna a los roles. */
                ComponenteManager componenteManager = new ComponenteManager();

                /* Crea los roles si no existen */
                if (rolDAO.GetAll().Count == 0)
                {
                    rolDAO.Insert(new Rol { Nombre = "Dueño" });
                    rolDAO.Insert(new Rol { Nombre = "EncargadoVentas" });
                    rolDAO.Insert(new Rol { Nombre = "EmpleadoVentas" });
                    rolDAO.Insert(new Rol { Nombre = "EncargadoCompras" });
                    rolDAO.Insert(new Rol { Nombre = "EncargadoInventario" });
                }

                /* Crea el árbol de permisos si todavía no existe y se lo asigna a cada rol.
                   Se ejecuta ANTES de verificar los usuarios para que también funcione cuando el sistema
                   ya estaba en uso y los roles todavía no tenían permisos. Es seguro ejecutarlo
                   en cada arranque porque no duplica nada. */
                componenteManager.InicializarComponentes();

                /* Si ya hay usuarios no hace nada más */
                if (usuarioDAO.GetAllOrFiltered().Count > 0)
                    return;

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
                    "Usuario: JPEREZ\n" +
                    "Contraseña: Admin123!\n\n",
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
