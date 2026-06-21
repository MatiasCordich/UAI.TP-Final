using BLL.SECURITY;
using ENTITY;
using System;
using System.Windows.Forms;
using UI.Utils;

namespace UI
{
    /* -----------------------------------------------------------------------------------------------------
     * Formulario: FrmCambiarClave
     * Descripción: Formulario de cambio de clave obligatorio.
     *              Se abre cuando el usuario ingresa por primera vez, cuando su clave fue reseteada
     *              por el admin o cuando la clave venció (90 días).
     *              El usuario NO puede cerrar este formulario sin cambiar la clave.
     -----------------------------------------------------------------------------------------------------*/
    public partial class FrmCambiarClave : Form
    {
        /* Instancia del servicio de autenticación */
        private AuthService authService = new AuthService();

        /* Usuario que debe cambiar la clave */
        private Usuario usuario;

        public FrmCambiarClave(Usuario usuario)
        {
            InitializeComponent();

            /* Guarda el usuario que debe cambiar la clave */
            this.usuario = usuario;
        }

        /* -----------------------------------------------------------------------------------------------------
         * Evento: FrmCambiarClave_Load
         * Descripción: Se ejecuta al cargar el formulario.
         -----------------------------------------------------------------------------------------------------*/
        private void FrmCambiarClave_Load(object sender, EventArgs e)
        {
            /* Pone el foco en el campo de clave actual */
            txtClaveActual.Focus();
        }

        /* -----------------------------------------------------------------------------------------------------
         * Evento: btnCambiar_Click
         * Descripción: Valida y cambia la clave del usuario.
         *              Si el cambio es exitoso abre el formulario principal.
         -----------------------------------------------------------------------------------------------------*/
        private void btnCambiar_Click(object sender, EventArgs e)
        {
            try
            {
                /* Valida que todos los campos estén completos */
                if (string.IsNullOrEmpty(txtClaveActual.Text))
                {
                    DialogHelper.MostrarAviso("Debe ingresar su contraseña actual.");
                    txtClaveActual.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(txtNuevaClave.Text))
                {
                    DialogHelper.MostrarAviso("Debe ingresar la nueva contraseña.");
                    txtNuevaClave.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(txtConfirmarClave.Text))
                {
                    DialogHelper.MostrarAviso("Debe confirmar la nueva contraseña.");
                    txtConfirmarClave.Focus();
                    return;
                }

                /* Valida que la nueva clave y la confirmación coincidan */
                if (txtNuevaClave.Text != txtConfirmarClave.Text)
                {
                    DialogHelper.MostrarAviso("La nueva contraseña y la confirmación no coinciden.");
                    txtNuevaClave.Clear();
                    txtConfirmarClave.Clear();
                    txtNuevaClave.Focus();
                    return;
                }

                /* Llama al AuthService para cambiar la clave */
                authService.CambiarClave(usuario, txtClaveActual.Text, txtNuevaClave.Text);

                /* Informa al usuario que el cambio fue exitoso */
                DialogHelper.MostrarExito("Contraseña cambiada correctamente. Ya puede ingresar al sistema.");

                /* Guarda el usuario en sesión ahora que ya cambió la clave */
                authService.GuardarSesion(usuario);

                /* Abre el formulario principal */
                FrmPrincipal principal = new FrmPrincipal(usuario);
                principal.Show();

                /* Cierra este formulario */
                this.Close();
            }
            catch (Exception ex)
            {
                DialogHelper.MostrarError(ex.Message);

                /* Limpia los campos de clave nueva y confirmación */
                txtNuevaClave.Clear();
                txtConfirmarClave.Clear();
                txtNuevaClave.Focus();
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Evento: btnVerClaveActual_Click
         * Descripción: Muestra u oculta la clave actual.
         -----------------------------------------------------------------------------------------------------*/
        private void btnVerClaveActual_Click(object sender, EventArgs e)
        {
            /* Si la contraseña actual está oculta la muestra */
            if (txtClaveActual.UseSystemPasswordChar)
            {
                /* Muestra la contraseña actual en texto plano */
                txtClaveActual.UseSystemPasswordChar = false;

                /* Se cambia el texto del botón */
                btnVerClaveActual.Text = "Ocultar";
            }
            else
            {
                /* Oculta la contraseña nuevamente */
                txtClaveActual.UseSystemPasswordChar = true;

                /* Se cambia el texto del botón */
                btnVerClaveActual.Text = "Mostrar";
            }

            /* Mantiene el foco en el campo de contraseña */
            txtClaveActual.Focus();
        }

        /* -----------------------------------------------------------------------------------------------------
         * Evento: btnVerClaveNueva_Click
         * Descripción: Muestra u oculta la nueva clave.
         -----------------------------------------------------------------------------------------------------*/
        private void btnVerClaveNueva_Click(object sender, EventArgs e)
        {
            /* Si la contraseña actual está oculta la muestra */
            if (txtNuevaClave.UseSystemPasswordChar)
            {
                /* Muestra la contraseña actual en texto plano */
                txtNuevaClave.UseSystemPasswordChar = false;

                /* Se cambia el texto del botón */
                btnVerClaveNueva.Text = "Ocultar";
            }
            else
            {
                /* Oculta la contraseña nuevamente */
                txtNuevaClave.UseSystemPasswordChar = true;

                /* Se cambia el texto del botón */
                btnVerClaveNueva.Text = "Mostrar";
            }

            /* Mantiene el foco en el campo de contraseña */
            txtNuevaClave.Focus();
        }

        /* -----------------------------------------------------------------------------------------------------
         * Evento: btnVerConfirmarClave_Click
         * Descripción: Muestra u oculta la confirmación de clave.
         -----------------------------------------------------------------------------------------------------*/
        private void btnVerConfirmarClave_Click(object sender, EventArgs e)
        {
            /* Si la contraseña actual está oculta la muestra */
            if (txtConfirmarClave.UseSystemPasswordChar)
            {
                /* Muestra la contraseña actual en texto plano */
                txtConfirmarClave.UseSystemPasswordChar = false;

                /* Se cambia el texto del botón */
                btnVerConfirmarClave.Text = "Ocultar";
            }
            else
            {
                /* Oculta la contraseña nuevamente */
                txtConfirmarClave.UseSystemPasswordChar = true;

                /* Se cambia el texto del botón */
                btnVerConfirmarClave.Text = "Mostrar";
            }

            /* Mantiene el foco en el campo de contraseña */
            txtConfirmarClave.Focus();
        }
    }
}
