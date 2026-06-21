namespace UI
{
    partial class FrmCambiarClave
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtClaveActual = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtNuevaClave = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtConfirmarClave = new System.Windows.Forms.TextBox();
            this.btnCambiar = new System.Windows.Forms.Button();
            this.btnVerClaveActual = new System.Windows.Forms.Button();
            this.btnVerClaveNueva = new System.Windows.Forms.Button();
            this.btnVerConfirmarClave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Gadugi", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(46, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(264, 33);
            this.label1.TabIndex = 0;
            this.label1.Text = "Cambiar Contraseña";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Gadugi", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(48, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(425, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "Debe cambiar su contraseña antes de continuar.";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.BackColor = System.Drawing.Color.LemonChiffon;
            this.label3.Font = new System.Drawing.Font("Gadugi", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.label3.Location = new System.Drawing.Point(48, 120);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(4);
            this.label3.Size = new System.Drawing.Size(439, 105);
            this.label3.TabIndex = 3;
            this.label3.Text = "La contraseña debe tener mínimo 8 caracteres, una mayúscula, una minúscula, un nú" +
    "mero y un símbolo.";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Gadugi", 12F);
            this.label4.Location = new System.Drawing.Point(47, 245);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(202, 28);
            this.label4.TabIndex = 4;
            this.label4.Text = "Contraseña actual";
            // 
            // txtClaveActual
            // 
            this.txtClaveActual.Font = new System.Drawing.Font("Gadugi", 11F);
            this.txtClaveActual.Location = new System.Drawing.Point(52, 276);
            this.txtClaveActual.Name = "txtClaveActual";
            this.txtClaveActual.Size = new System.Drawing.Size(337, 37);
            this.txtClaveActual.TabIndex = 5;
            this.txtClaveActual.UseSystemPasswordChar = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Gadugi", 12F);
            this.label5.Location = new System.Drawing.Point(47, 337);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(208, 28);
            this.label5.TabIndex = 6;
            this.label5.Text = "Nueva Contraseña";
            // 
            // txtNuevaClave
            // 
            this.txtNuevaClave.Font = new System.Drawing.Font("Gadugi", 11F);
            this.txtNuevaClave.Location = new System.Drawing.Point(52, 368);
            this.txtNuevaClave.Name = "txtNuevaClave";
            this.txtNuevaClave.Size = new System.Drawing.Size(337, 37);
            this.txtNuevaClave.TabIndex = 7;
            this.txtNuevaClave.UseSystemPasswordChar = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Gadugi", 12F);
            this.label6.Location = new System.Drawing.Point(47, 432);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(241, 28);
            this.label6.TabIndex = 8;
            this.label6.Text = "Confirmar contraseña";
            // 
            // txtConfirmarClave
            // 
            this.txtConfirmarClave.Font = new System.Drawing.Font("Gadugi", 11F);
            this.txtConfirmarClave.Location = new System.Drawing.Point(52, 463);
            this.txtConfirmarClave.Name = "txtConfirmarClave";
            this.txtConfirmarClave.Size = new System.Drawing.Size(337, 37);
            this.txtConfirmarClave.TabIndex = 9;
            this.txtConfirmarClave.UseSystemPasswordChar = true;
            // 
            // btnCambiar
            // 
            this.btnCambiar.BackColor = System.Drawing.Color.CornflowerBlue;
            this.btnCambiar.Font = new System.Drawing.Font("Gadugi", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCambiar.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnCambiar.Location = new System.Drawing.Point(52, 524);
            this.btnCambiar.Name = "btnCambiar";
            this.btnCambiar.Size = new System.Drawing.Size(435, 52);
            this.btnCambiar.TabIndex = 10;
            this.btnCambiar.Text = "Cambiar";
            this.btnCambiar.UseVisualStyleBackColor = false;
            this.btnCambiar.Click += new System.EventHandler(this.btnCambiar_Click);
            // 
            // btnVerClaveActual
            // 
            this.btnVerClaveActual.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.btnVerClaveActual.Font = new System.Drawing.Font("Gadugi", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVerClaveActual.Location = new System.Drawing.Point(398, 276);
            this.btnVerClaveActual.Name = "btnVerClaveActual";
            this.btnVerClaveActual.Size = new System.Drawing.Size(89, 37);
            this.btnVerClaveActual.TabIndex = 11;
            this.btnVerClaveActual.Text = "Mostrar";
            this.btnVerClaveActual.UseVisualStyleBackColor = false;
            this.btnVerClaveActual.Click += new System.EventHandler(this.btnVerClaveActual_Click);
            // 
            // btnVerClaveNueva
            // 
            this.btnVerClaveNueva.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.btnVerClaveNueva.Font = new System.Drawing.Font("Gadugi", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVerClaveNueva.Location = new System.Drawing.Point(398, 368);
            this.btnVerClaveNueva.Name = "btnVerClaveNueva";
            this.btnVerClaveNueva.Size = new System.Drawing.Size(89, 37);
            this.btnVerClaveNueva.TabIndex = 12;
            this.btnVerClaveNueva.Text = "Mostrar";
            this.btnVerClaveNueva.UseVisualStyleBackColor = false;
            this.btnVerClaveNueva.Click += new System.EventHandler(this.btnVerClaveNueva_Click);
            // 
            // btnVerConfirmarClave
            // 
            this.btnVerConfirmarClave.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.btnVerConfirmarClave.Font = new System.Drawing.Font("Gadugi", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVerConfirmarClave.Location = new System.Drawing.Point(398, 463);
            this.btnVerConfirmarClave.Name = "btnVerConfirmarClave";
            this.btnVerConfirmarClave.Size = new System.Drawing.Size(89, 37);
            this.btnVerConfirmarClave.TabIndex = 13;
            this.btnVerConfirmarClave.Text = "Mostrar";
            this.btnVerConfirmarClave.UseVisualStyleBackColor = false;
            this.btnVerConfirmarClave.Click += new System.EventHandler(this.btnVerConfirmarClave_Click);
            // 
            // FrmCambiarClave
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 659);
            this.ControlBox = false;
            this.Controls.Add(this.btnVerConfirmarClave);
            this.Controls.Add(this.btnVerClaveNueva);
            this.Controls.Add(this.btnVerClaveActual);
            this.Controls.Add(this.btnCambiar);
            this.Controls.Add(this.txtConfirmarClave);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtNuevaClave);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtClaveActual);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCambiarClave";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cambiar Contraseña";
            this.Load += new System.EventHandler(this.FrmCambiarClave_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtClaveActual;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtNuevaClave;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtConfirmarClave;
        private System.Windows.Forms.Button btnCambiar;
        private System.Windows.Forms.Button btnVerClaveActual;
        private System.Windows.Forms.Button btnVerClaveNueva;
        private System.Windows.Forms.Button btnVerConfirmarClave;
    }
}