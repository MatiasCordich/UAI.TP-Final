namespace UI
{
    partial class FrmLogin
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
            this.GbLogin = new System.Windows.Forms.GroupBox();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.lblClave = new System.Windows.Forms.Label();
            this.txtClave = new System.Windows.Forms.TextBox();
            this.btnIngresar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.GbLogin.SuspendLayout();
            this.SuspendLayout();
            // 
            // GbLogin
            // 
            this.GbLogin.Controls.Add(this.btnCancelar);
            this.GbLogin.Controls.Add(this.btnIngresar);
            this.GbLogin.Controls.Add(this.txtClave);
            this.GbLogin.Controls.Add(this.lblClave);
            this.GbLogin.Controls.Add(this.txtUsuario);
            this.GbLogin.Controls.Add(this.lblUsuario);
            this.GbLogin.Font = new System.Drawing.Font("Gadugi", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GbLogin.ForeColor = System.Drawing.Color.Maroon;
            this.GbLogin.Location = new System.Drawing.Point(50, 37);
            this.GbLogin.Name = "GbLogin";
            this.GbLogin.Size = new System.Drawing.Size(737, 293);
            this.GbLogin.TabIndex = 0;
            this.GbLogin.TabStop = false;
            this.GbLogin.Text = "Login";
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Font = new System.Drawing.Font("Gadugi", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsuario.ForeColor = System.Drawing.Color.Black;
            this.lblUsuario.Location = new System.Drawing.Point(63, 84);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(100, 28);
            this.lblUsuario.TabIndex = 0;
            this.lblUsuario.Text = "Usuario";
            // 
            // txtUsuario
            // 
            this.txtUsuario.Font = new System.Drawing.Font("Gadugi", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsuario.Location = new System.Drawing.Point(215, 76);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(450, 50);
            this.txtUsuario.TabIndex = 1;
            // 
            // lblClave
            // 
            this.lblClave.AutoSize = true;
            this.lblClave.Font = new System.Drawing.Font("Gadugi", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClave.ForeColor = System.Drawing.Color.Black;
            this.lblClave.Location = new System.Drawing.Point(63, 154);
            this.lblClave.Name = "lblClave";
            this.lblClave.Size = new System.Drawing.Size(141, 28);
            this.lblClave.TabIndex = 2;
            this.lblClave.Text = "Contraseña";
            // 
            // txtClave
            // 
            this.txtClave.Font = new System.Drawing.Font("Gadugi", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtClave.Location = new System.Drawing.Point(215, 145);
            this.txtClave.Name = "txtClave";
            this.txtClave.Size = new System.Drawing.Size(450, 50);
            this.txtClave.TabIndex = 3;
            this.txtClave.UseSystemPasswordChar = true;
            this.txtClave.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtClave_KeyPress);
            // 
            // btnIngresar
            // 
            this.btnIngresar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnIngresar.Font = new System.Drawing.Font("Gadugi", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIngresar.ForeColor = System.Drawing.Color.Black;
            this.btnIngresar.Location = new System.Drawing.Point(68, 214);
            this.btnIngresar.Name = "btnIngresar";
            this.btnIngresar.Size = new System.Drawing.Size(270, 59);
            this.btnIngresar.TabIndex = 4;
            this.btnIngresar.Text = "Ingresar";
            this.btnIngresar.UseVisualStyleBackColor = false;
            this.btnIngresar.Click += new System.EventHandler(this.btnIngresar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.Color.Silver;
            this.btnCancelar.Font = new System.Drawing.Font("Gadugi", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.ForeColor = System.Drawing.Color.Black;
            this.btnCancelar.Location = new System.Drawing.Point(395, 214);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(270, 59);
            this.btnCancelar.TabIndex = 5;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // FrmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(868, 381);
            this.Controls.Add(this.GbLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Babilonia Calzados - Iniciar Sesión";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmLogin_FormClosed);
            this.GbLogin.ResumeLayout(false);
            this.GbLogin.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GbLogin;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.TextBox txtClave;
        private System.Windows.Forms.Label lblClave;
        private System.Windows.Forms.Button btnIngresar;
        private System.Windows.Forms.Button btnCancelar;
    }
}