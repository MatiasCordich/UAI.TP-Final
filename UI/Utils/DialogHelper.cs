using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI.Utils
{
    /* -----------------------------------------------------------------------------------------------------
     * Clase: DialogHelper
     * Descripción: Clase utilitaria para mostrar diálogos de confirmación en español.
     *              WinForms no permite cambiar el texto de los botones de MessageBox nativamente,
     *              por lo que esta clase crea un formulario personalizado con botones en español.
     -----------------------------------------------------------------------------------------------------*/
    public static class DialogHelper
    {
        /* -----------------------------------------------------------------------------------------------------
         * Función: MostrarConfirmacion
         * Descripción: Muestra un diálogo de confirmación con botones "Sí" y "No" en español.
         * Parámetros: mensaje a mostrar y título del diálogo.
         * Retorna: DialogResult.Yes si el usuario confirma, DialogResult.No si cancela.
         -----------------------------------------------------------------------------------------------------*/
        public static DialogResult MostrarConfirmacion(string mensaje, string titulo)
        {
            /* Crea el formulario del diálogo */
            Form dialogo = new Form();
            dialogo.Text = titulo;
            dialogo.Size = new Size(390, 150);
            dialogo.StartPosition = FormStartPosition.CenterParent;
            dialogo.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            dialogo.MaximizeBox = false;
            dialogo.MinimizeBox = false;
           

            /* Ícono de pregunta */
            PictureBox icono = new PictureBox();
            icono.Image = SystemIcons.Question.ToBitmap();
            icono.Size = new Size(32, 32);
            icono.Location = new Point(16, 20);
            icono.SizeMode = PictureBoxSizeMode.StretchImage;

            /* Etiqueta con el mensaje */
            Label lblMensaje = new Label();
            lblMensaje.Text = mensaje;
            lblMensaje.AutoSize = false;
            lblMensaje.Size = new Size(300, 50);
            lblMensaje.Location = new Point(58, 10);
            lblMensaje.TextAlign = ContentAlignment.MiddleLeft;
            lblMensaje.Font = new Font("Segoe UI", 9f);

            /* Botón Sí */
            Button btnSi = new Button();
            btnSi.Text = "Sí";
            btnSi.DialogResult = DialogResult.Yes;
            btnSi.Location = new Point(195, 70);
            btnSi.Size = new Size(80, 30);
            btnSi.Font = new Font("Segoe UI", 9f);

            /* Botón No */
            Button btnNo = new Button();
            btnNo.Text = "No";
            btnNo.DialogResult = DialogResult.No;
            btnNo.Location = new Point(285, 70);
            btnNo.Size = new Size(80, 30);
            btnNo.Font = new Font("Segoe UI", 9f);

            /* Agrega los controles al diálogo */
            dialogo.Controls.Add(icono);
            dialogo.Controls.Add(lblMensaje);
            dialogo.Controls.Add(btnSi);
            dialogo.Controls.Add(btnNo);

            /* Enter confirma, Escape cancela */
            dialogo.AcceptButton = btnSi;
            dialogo.CancelButton = btnNo;

            /* Muestra el diálogo y devuelve el resultado */
            return dialogo.ShowDialog();
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: MostrarError
         * Descripción: Muestra un mensaje de error estándar.
         * Parámetros: mensaje de error.
         -----------------------------------------------------------------------------------------------------*/
        public static void MostrarError(string mensaje)
        {
            MessageBox.Show(mensaje,
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: MostrarAviso
         * Descripción: Muestra un mensaje de aviso estándar.
         * Parámetros: mensaje de aviso.
         -----------------------------------------------------------------------------------------------------*/
        public static void MostrarAviso(string mensaje)
        {
            MessageBox.Show(mensaje,
                            "Aviso",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: MostrarExito
         * Descripción: Muestra un mensaje de éxito estándar.
         * Parámetros: mensaje de éxito.
         -----------------------------------------------------------------------------------------------------*/
        public static void MostrarExito(string mensaje)
        {
            MessageBox.Show(mensaje,
                            "Éxito",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
        }
    }
}
