using BLL.SECURITY;
using ENTITY;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
     /* -----------------------------------------------------------------------------------------------------
     * Formulario: FrmLogin
     * Descripción: Pantalla de inicio de sesión del sistema Babilonia Calzados (Desafío I).
     *              Identifica y autentica al usuario antes de permitir el acceso.
     *              Si las credenciales son correctas abre el formulario principal.
     -----------------------------------------------------------------------------------------------------*/
    public partial class FrmLogin : Form
    {
        /* Instancia del servicio de autenticación */
        private AuthService authService = new AuthService();

        public FrmLogin()
        {
            /* Inicializa los componentes del formulario */
            InitializeComponent();
        }

        /* -----------------------------------------------------------------------------------------------------
         * Evento: FrmLogin_FormClosed
         * Descripción: Se ejecuta al cerrar el formulario de login.
         *              Si se cierra sin haber iniciado sesión termina la aplicación.
         -----------------------------------------------------------------------------------------------------*/
        private void FrmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            /* Si no hay usuario logueado cierra la aplicación */
            if (AuthService.UsuarioActual == null) {
                Application.Exit();
            } 
                
        }

        /* -----------------------------------------------------------------------------------------------------
         * Evento: btnIngresar_Click
         * Descripción: Se ejecuta al hacer clic en el botón Ingresar.
         *              Valida las credenciales y abre el formulario principal si son correctas.
         -----------------------------------------------------------------------------------------------------*/
        private void btnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                /* Valida que el campo usuario no esté vacío */
                if (string.IsNullOrEmpty(txtUsuario.Text))
                {
                    MessageBox.Show("Debe ingresar su nombre de usuario.",
                                    "Campo requerido",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);

                    /* Funcion Focus() para que el se ubique en el input del usuario. */
                    txtUsuario.Focus();
                    return;
                }

                /* Valida que el campo contraseña no esté vacío */
                if (string.IsNullOrEmpty(txtClave.Text))
                {
                    MessageBox.Show("Debe ingresar su contraseña.",
                                    "Campo requerido",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);

                    /* Funcion Focus() para que el se ubique en el input de la contraseña. */
                    txtClave.Focus();
                    return;
                }

                /* Intenta autenticar al usuario con las credenciales ingresadas */
                Usuario usuario = authService.Login(txtUsuario.Text.Trim(), txtClave.Text);

                /* Si la autenticación fue exitosa abre el formulario principal */
                FrmPrincipal principal = new FrmPrincipal(usuario);
                principal.Show();

                /* Cierra el formulario de login */
                this.Hide();
            }
            catch (Exception ex)
            {
                /* Muestra el mensaje de error devuelto por el AuthService */
                MessageBox.Show(ex.Message,
                                "Error de acceso",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                /* Limpia la contraseña y pone el foco en ella para reintentar */
                txtUsuario.Clear();
                txtClave.Focus();
            }

        }

        /* -----------------------------------------------------------------------------------------------------
         * Evento: btnCancelar_Click
         * Descripción: Se ejecuta al hacer clic en el botón Cancelar.
         *              Cierra la aplicación.
         -----------------------------------------------------------------------------------------------------*/
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            /* Cierra la aplicación completamente */
            Application.Exit();
        }

        /* -----------------------------------------------------------------------------------------------------
        * Evento: txtClave_KeyPress
        * Descripción: Permite presionar Enter en el campo contraseña para iniciar sesión
        *              sin necesidad de hacer clic en el botón.
        -----------------------------------------------------------------------------------------------------*/
        private void txtClave_KeyPress(object sender, KeyPressEventArgs e)
        {
            /* Si se presiona Enter ejecuta el login */
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnIngresar_Click(sender, e);
            }
                
        }

        /* -----------------------------------------------------------------------------------------------------
        * Evento: btnVerClave_Click
        * Descripción: Permite ver la constraseña o ocultarla. 
        -----------------------------------------------------------------------------------------------------*/
        private void btnVerClave_Click(object sender, EventArgs e)
        {
            /* Si la contraseña está oculta la muestra */
            if (txtClave.UseSystemPasswordChar)
            {
                /* Muestra la contraseña en texto plano */
                txtClave.UseSystemPasswordChar = false;

                /* Se cambia el texto del botón */
                btnVerClave.Text = "Ocultar";
            }
            else
            {
                /* Oculta la contraseña nuevamente */
                txtClave.UseSystemPasswordChar = true;

                /* Se cambia el texto del botón */
                btnVerClave.Text = "Mostrar";
            }

            /* Mantiene el foco en el campo de contraseña */
            txtClave.Focus();
        }
    }
}
