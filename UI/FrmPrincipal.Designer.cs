namespace UI
{
    partial class FrmPrincipal
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
            this.Menu_strip = new System.Windows.Forms.MenuStrip();
            this.MenuVentas = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuNuevaVenta = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuPostVenta = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuClientes = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuCompras = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuPedidos = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuRecepcion = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuPagos = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuRegistrarPago = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuConciliacion = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuStock = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuAjusteStock = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuAlertas = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuFidelizacion = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuCupones = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuReportes = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuDashboard = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuAdministracion = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuUsuarios = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuProductos = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuProveedores = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSesión = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuCerrarSesion = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuSalirAplicacion = new System.Windows.Forms.ToolStripMenuItem();
            this.Menu_strip.SuspendLayout();
            this.SuspendLayout();
            // 
            // Menu_strip
            // 
            this.Menu_strip.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.Menu_strip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.Menu_strip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuVentas,
            this.MenuCompras,
            this.MenuPagos,
            this.MenuStock,
            this.MenuFidelizacion,
            this.MenuReportes,
            this.MenuAdministracion,
            this.MenuSesión});
            this.Menu_strip.Location = new System.Drawing.Point(0, 0);
            this.Menu_strip.Name = "Menu_strip";
            this.Menu_strip.Padding = new System.Windows.Forms.Padding(10, 3, 0, 3);
            this.Menu_strip.Size = new System.Drawing.Size(1402, 40);
            this.Menu_strip.TabIndex = 1;
            this.Menu_strip.Text = "menuStrip1";
            // 
            // MenuVentas
            // 
            this.MenuVentas.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuNuevaVenta,
            this.MenuPostVenta,
            this.MenuClientes});
            this.MenuVentas.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MenuVentas.Name = "MenuVentas";
            this.MenuVentas.Size = new System.Drawing.Size(93, 34);
            this.MenuVentas.Text = "Ventas";
            // 
            // MenuNuevaVenta
            // 
            this.MenuNuevaVenta.Name = "MenuNuevaVenta";
            this.MenuNuevaVenta.Size = new System.Drawing.Size(270, 38);
            this.MenuNuevaVenta.Text = "Nueva Venta";
            this.MenuNuevaVenta.Click += new System.EventHandler(this.MenuNuevaVenta_Click);
            // 
            // MenuPostVenta
            // 
            this.MenuPostVenta.Name = "MenuPostVenta";
            this.MenuPostVenta.Size = new System.Drawing.Size(270, 38);
            this.MenuPostVenta.Text = "Post Venta";
            this.MenuPostVenta.Click += new System.EventHandler(this.MenuPostVenta_Click);
            // 
            // MenuClientes
            // 
            this.MenuClientes.Name = "MenuClientes";
            this.MenuClientes.Size = new System.Drawing.Size(270, 38);
            this.MenuClientes.Text = "Clientes";
            this.MenuClientes.Click += new System.EventHandler(this.MenuClientes_Click);
            // 
            // MenuCompras
            // 
            this.MenuCompras.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuPedidos,
            this.MenuRecepcion});
            this.MenuCompras.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MenuCompras.Name = "MenuCompras";
            this.MenuCompras.Size = new System.Drawing.Size(116, 34);
            this.MenuCompras.Text = "Compras";
            // 
            // MenuPedidos
            // 
            this.MenuPedidos.Name = "MenuPedidos";
            this.MenuPedidos.Size = new System.Drawing.Size(270, 38);
            this.MenuPedidos.Text = "Pedidos";
            this.MenuPedidos.Click += new System.EventHandler(this.MenuPedidos_Click);
            // 
            // MenuRecepcion
            // 
            this.MenuRecepcion.Name = "MenuRecepcion";
            this.MenuRecepcion.Size = new System.Drawing.Size(270, 38);
            this.MenuRecepcion.Text = "Recepción";
            this.MenuRecepcion.Click += new System.EventHandler(this.MenuRecepcion_Click);
            // 
            // MenuPagos
            // 
            this.MenuPagos.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuRegistrarPago,
            this.MenuConciliacion});
            this.MenuPagos.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MenuPagos.Name = "MenuPagos";
            this.MenuPagos.Size = new System.Drawing.Size(86, 34);
            this.MenuPagos.Text = "Pagos";
            // 
            // MenuRegistrarPago
            // 
            this.MenuRegistrarPago.Name = "MenuRegistrarPago";
            this.MenuRegistrarPago.Size = new System.Drawing.Size(270, 38);
            this.MenuRegistrarPago.Text = "Registrar Pago";
            this.MenuRegistrarPago.Click += new System.EventHandler(this.MenuRegistrarPago_Click);
            // 
            // MenuConciliacion
            // 
            this.MenuConciliacion.Name = "MenuConciliacion";
            this.MenuConciliacion.Size = new System.Drawing.Size(270, 38);
            this.MenuConciliacion.Text = "Conciliación";
            this.MenuConciliacion.Click += new System.EventHandler(this.MenuConciliacion_Click);
            // 
            // MenuStock
            // 
            this.MenuStock.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuAjusteStock,
            this.MenuAlertas});
            this.MenuStock.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.MenuStock.Name = "MenuStock";
            this.MenuStock.Size = new System.Drawing.Size(81, 34);
            this.MenuStock.Text = "Stock";
            // 
            // MenuAjusteStock
            // 
            this.MenuAjusteStock.Name = "MenuAjusteStock";
            this.MenuAjusteStock.Size = new System.Drawing.Size(270, 38);
            this.MenuAjusteStock.Text = "Ajuste de Stock";
            this.MenuAjusteStock.Click += new System.EventHandler(this.MenuAjusteStock_Click);
            // 
            // MenuAlertas
            // 
            this.MenuAlertas.Name = "MenuAlertas";
            this.MenuAlertas.Size = new System.Drawing.Size(270, 38);
            this.MenuAlertas.Text = "Alertas";
            this.MenuAlertas.Click += new System.EventHandler(this.MenuAlertas_Click);
            // 
            // MenuFidelizacion
            // 
            this.MenuFidelizacion.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuCupones});
            this.MenuFidelizacion.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.MenuFidelizacion.Name = "MenuFidelizacion";
            this.MenuFidelizacion.Size = new System.Drawing.Size(141, 34);
            this.MenuFidelizacion.Text = "Fidelización";
            // 
            // MenuCupones
            // 
            this.MenuCupones.Name = "MenuCupones";
            this.MenuCupones.Size = new System.Drawing.Size(270, 38);
            this.MenuCupones.Text = "Cupones";
            this.MenuCupones.Click += new System.EventHandler(this.MenuCupones_Click);
            // 
            // MenuReportes
            // 
            this.MenuReportes.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuDashboard});
            this.MenuReportes.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.MenuReportes.Name = "MenuReportes";
            this.MenuReportes.Size = new System.Drawing.Size(115, 34);
            this.MenuReportes.Text = "Reportes";
            // 
            // MenuDashboard
            // 
            this.MenuDashboard.Name = "MenuDashboard";
            this.MenuDashboard.Size = new System.Drawing.Size(270, 38);
            this.MenuDashboard.Text = "Dashboard";
            this.MenuDashboard.Click += new System.EventHandler(this.MenuDashboard_Click);
            // 
            // MenuAdministracion
            // 
            this.MenuAdministracion.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuUsuarios,
            this.MenuProductos,
            this.MenuProveedores});
            this.MenuAdministracion.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.MenuAdministracion.Name = "MenuAdministracion";
            this.MenuAdministracion.Size = new System.Drawing.Size(172, 34);
            this.MenuAdministracion.Text = "Administración";
            // 
            // MenuUsuarios
            // 
            this.MenuUsuarios.Name = "MenuUsuarios";
            this.MenuUsuarios.Size = new System.Drawing.Size(270, 38);
            this.MenuUsuarios.Text = "Usuarios";
            this.MenuUsuarios.Click += new System.EventHandler(this.MenuUsuarios_Click);
            // 
            // MenuProductos
            // 
            this.MenuProductos.Name = "MenuProductos";
            this.MenuProductos.Size = new System.Drawing.Size(270, 38);
            this.MenuProductos.Text = "Productos";
            this.MenuProductos.Click += new System.EventHandler(this.MenuProductos_Click);
            // 
            // MenuProveedores
            // 
            this.MenuProveedores.Name = "MenuProveedores";
            this.MenuProveedores.Size = new System.Drawing.Size(270, 38);
            this.MenuProveedores.Text = "Proveedores";
            this.MenuProveedores.Click += new System.EventHandler(this.MenuProveedores_Click);
            // 
            // MenuSesión
            // 
            this.MenuSesión.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuCerrarSesion,
            this.MenuSalirAplicacion});
            this.MenuSesión.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MenuSesión.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.MenuSesión.Name = "MenuSesión";
            this.MenuSesión.Size = new System.Drawing.Size(95, 34);
            this.MenuSesión.Text = "Sesión";
            // 
            // MenuCerrarSesion
            // 
            this.MenuCerrarSesion.ForeColor = System.Drawing.Color.Red;
            this.MenuCerrarSesion.Name = "MenuCerrarSesion";
            this.MenuCerrarSesion.Size = new System.Drawing.Size(314, 38);
            this.MenuCerrarSesion.Text = "Cerrar Sesión";
            this.MenuCerrarSesion.Click += new System.EventHandler(this.MenuCerrarSesion_Click);
            // 
            // MenuSalirAplicacion
            // 
            this.MenuSalirAplicacion.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MenuSalirAplicacion.Name = "MenuSalirAplicacion";
            this.MenuSalirAplicacion.Size = new System.Drawing.Size(314, 38);
            this.MenuSalirAplicacion.Text = "Salir de la Aplicación";
            this.MenuSalirAplicacion.Click += new System.EventHandler(this.MenuSalirAplicacion_Click);
            // 
            // FrmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1402, 800);
            this.ControlBox = false;
            this.Controls.Add(this.Menu_strip);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.Menu_strip;
            this.Name = "FrmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Babilonia Calzados";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmPrincipal_Load);
            this.Menu_strip.ResumeLayout(false);
            this.Menu_strip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip Menu_strip;
        private System.Windows.Forms.ToolStripMenuItem MenuVentas;
        private System.Windows.Forms.ToolStripMenuItem MenuCompras;
        private System.Windows.Forms.ToolStripMenuItem MenuPagos;
        private System.Windows.Forms.ToolStripMenuItem MenuStock;
        private System.Windows.Forms.ToolStripMenuItem MenuFidelizacion;
        private System.Windows.Forms.ToolStripMenuItem MenuReportes;
        private System.Windows.Forms.ToolStripMenuItem MenuAdministracion;
        private System.Windows.Forms.ToolStripMenuItem MenuSesión;
        private System.Windows.Forms.ToolStripMenuItem MenuNuevaVenta;
        private System.Windows.Forms.ToolStripMenuItem MenuPostVenta;
        private System.Windows.Forms.ToolStripMenuItem MenuClientes;
        private System.Windows.Forms.ToolStripMenuItem MenuPedidos;
        private System.Windows.Forms.ToolStripMenuItem MenuRecepcion;
        private System.Windows.Forms.ToolStripMenuItem MenuRegistrarPago;
        private System.Windows.Forms.ToolStripMenuItem MenuConciliacion;
        private System.Windows.Forms.ToolStripMenuItem MenuAjusteStock;
        private System.Windows.Forms.ToolStripMenuItem MenuAlertas;
        private System.Windows.Forms.ToolStripMenuItem MenuCupones;
        private System.Windows.Forms.ToolStripMenuItem MenuDashboard;
        private System.Windows.Forms.ToolStripMenuItem MenuUsuarios;
        private System.Windows.Forms.ToolStripMenuItem MenuProductos;
        private System.Windows.Forms.ToolStripMenuItem MenuProveedores;
        private System.Windows.Forms.ToolStripMenuItem MenuCerrarSesion;
        private System.Windows.Forms.ToolStripMenuItem MenuSalirAplicacion;
    }
}

