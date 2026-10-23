using BLL.SECURITY;
using ENTITY;
using System;
using System.Windows.Forms;
using UI.Administracion;
using UI.Compras;
using UI.Fidelizacion;
using UI.Pagos;
using UI.Reportes;
using UI.Stock;
using UI.Utils;
using UI.Ventas;

namespace UI
{
    /* -----------------------------------------------------------------------------------------------------
     * Formulario: FrmPrincipal
     * Descripción: Formulario MDI principal del sistema Babilonia Calzados.
     *              Contiene el menú de navegación y controla el acceso según el rol del usuario.
     *              Todos los subformularios se abren dentro de este como MDI child.
     -----------------------------------------------------------------------------------------------------*/
    public partial class FrmPrincipal : Form
    {
        /* Usuario actualmente logueado en el sistema */
        private Usuario usuarioActual;

        public FrmPrincipal(Usuario usuario)
        {
            /* Inicializa los componentes del formulario */
            InitializeComponent();

            /* Guarda el usuario logueado */
            usuarioActual = usuario;
        }

        /* -----------------------------------------------------------------------------------------------------
        * Evento: FrmPrincipal_Load
        * Descripción: Se ejecuta al cargar el formulario principal.
        *              Configura el menú según el rol del usuario logueado.
        -----------------------------------------------------------------------------------------------------*/
        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            /* Muestra el nombre del usuario logueado en la barra de título */
            this.Text = $"Babilonia Calzados - {usuarioActual.Nombre} {usuarioActual.Apellido} [{usuarioActual.Rol?.Nombre}]";

            /* Configura la visibilidad del menú según el rol */
            ConfigurarMenuPorRol();
        }

        /* -----------------------------------------------------------------------------------------------------
        * Función: ConfigurarMenuPorRol
        * Descripción: Muestra u oculta las opciones del menú según los permisos del usuario logueado.
        *              Cada ítem se muestra solo si el árbol de permisos de su rol incluye la patente
        *              correspondiente (patrón Composite). Un menú principal se muestra si al menos
        *              uno de sus ítems está visible.
        -----------------------------------------------------------------------------------------------------*/
        private void ConfigurarMenuPorRol()
        {
            /* Se instancia el servicio de autenticación para consultar los permisos del usuario logueado. */
            AuthService authService = new AuthService();

            /* Se consulta cada permiso una sola vez y se guarda el resultado.
               No se lee la propiedad Visible de los ítems porque en un menú desplegable cerrado
               puede devolver false aunque el ítem esté habilitado para mostrarse. */

            /* VENTAS */
            bool nuevaVenta = authService.TienePermiso(NombresLeaf.NuevaVenta);
            bool postVenta = authService.TienePermiso(NombresLeaf.PostVenta);
            bool clientes = authService.TienePermiso(NombresLeaf.GestionarClientes);

            /* COMPRAS */
            bool pedidos = authService.TienePermiso(NombresLeaf.Pedidos);
            bool recepcion = authService.TienePermiso(NombresLeaf.Recepcion);

            /* PAGOS */
            bool registrarPago = authService.TienePermiso(NombresLeaf.RegistrarPago);
            bool conciliacion = authService.TienePermiso(NombresLeaf.Conciliacion);

            /* STOCK */
            bool ajusteStock = authService.TienePermiso(NombresLeaf.AjusteStock);
            bool alertas = authService.TienePermiso(NombresLeaf.Alertas);

            /* FIDELIZACIÓN */
            bool cupones = authService.TienePermiso(NombresLeaf.Cupones);

            /* REPORTES */
            bool dashboard = authService.TienePermiso(NombresLeaf.Dashboard);

            /* ADMINISTRACIÓN */
            bool usuarios = authService.TienePermiso(NombresLeaf.GestionarUsuarios);
            bool productos = authService.TienePermiso(NombresLeaf.GestionarProductos);
            bool proveedores = authService.TienePermiso(NombresLeaf.GestionarProveedores);

            /* Se muestra u oculta cada ítem del menú según su permiso. */
            MenuNuevaVenta.Visible = nuevaVenta;
            MenuPostVenta.Visible = postVenta;
            MenuClientes.Visible = clientes;
            MenuPedidos.Visible = pedidos;
            MenuRecepcion.Visible = recepcion;
            MenuRegistrarPago.Visible = registrarPago;
            MenuConciliacion.Visible = conciliacion;
            MenuAjusteStock.Visible = ajusteStock;
            MenuAlertas.Visible = alertas;
            MenuCupones.Visible = cupones;
            MenuDashboard.Visible = dashboard;
            MenuUsuarios.Visible = usuarios;
            MenuProductos.Visible = productos;
            MenuProveedores.Visible = proveedores;

            /* Se muestra cada menú principal solo si tiene al menos un ítem visible. */
            MenuVentas.Visible = nuevaVenta || postVenta || clientes;
            MenuCompras.Visible = pedidos || recepcion;
            MenuPagos.Visible = registrarPago || conciliacion;
            MenuStock.Visible = ajusteStock || alertas;
            MenuFidelizacion.Visible = cupones;
            MenuReportes.Visible = dashboard;
            MenuAdministracion.Visible = usuarios || productos || proveedores;
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: AbrirFormulario
         * Descripción: Abre un formulario como MDI child dentro del formulario principal.
         *              Si el formulario ya está abierto lo trae al frente en lugar de abrirlo de nuevo.
         * Parámetros: formulario a abrir.
         -----------------------------------------------------------------------------------------------------*/
        private void AbrirFormulario(Form formulario)
        {
            /* Busca si el formulario ya está abierto */
            foreach (Form f in this.MdiChildren)
            {
                /* Si ya está abierto lo trae al frente */
                if (f.GetType() == formulario.GetType())
                {
                    /* Funcion BringToFront() para traer al frente. */
                    f.BringToFront();
                    formulario.Dispose();
                    return;
                }
            }

            /* Si no está abierto lo configura como MDI child y lo muestra */
            formulario.MdiParent = this;
            formulario.Show();
        }

        /* =============================================
        * MENÚ: VENTAS
        * ============================================= */
        private void MenuNuevaVenta_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmNuevaVenta());
        }

        private void MenuPostVenta_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmPostVenta());
        }

        private void MenuClientes_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmCliente());
        }

        /* =============================================
        * MENÚ: COMPRAS
        * ============================================= */
        private void MenuPedidos_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmPedidos());
        }

        private void MenuRecepcion_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmRecepcion());
        }

        /* =============================================
        * MENÚ: PAGOS
        * ============================================= */
        private void MenuRegistrarPago_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmRegistrarPago());
        }

        private void MenuConciliacion_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmConciliacion());
        }

        /* =============================================
        * MENÚ: STOCK
        * ============================================= */
        private void MenuAjusteStock_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmAjusteStock());
        }

        private void MenuAlertas_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmAlertas());
        }

        /* =============================================
        * MENÚ: FIDELIZACIÓN
        * ============================================= */
        private void MenuCupones_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmCupones());
        }

        /* =============================================
        * MENÚ: REPORTES
        * ============================================= */
        private void MenuDashboard_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmDashboard());
        }

        /* =============================================
        * MENÚ: ADMINISTRACIÓN
        * ============================================= */
        private void MenuUsuarios_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmUsuarios());
        }

        private void MenuProductos_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmProductos());
        }

        private void MenuProveedores_Click(object sender, EventArgs e)
        {
            AbrirFormulario(new FrmProveedores());
        }

        /* =============================================
         * MENÚ: SESIÓN
         * ============================================= */

        /* -----------------------------------------------------------------------------------------------------
        * Evento: MenuCerrarSesion_Click
        * Descripción: Cierra la sesión del usuario actual y vuelve al login.
        -----------------------------------------------------------------------------------------------------*/
        private void MenuCerrarSesion_Click(object sender, EventArgs e)
        {
            /* Pide confirmación antes de cerrar sesión */
            DialogResult resultado = DialogHelper.MostrarConfirmacion(
                "¿Está seguro que desea cerrar sesión?",
                "Cerrar sesión");

            if (resultado == DialogResult.Yes)
            {
                /* Cierra la sesión del usuario */
                AuthService authService = new AuthService();
                authService.Logout();

                /* Cierra todos los formularios MDI child */
                foreach (Form f in this.MdiChildren)
                    f.Close();

                /* Muestra el formulario de login nuevamente */
                FrmLogin login = new FrmLogin();
                login.Show();

                /* Cierra el formulario principal */
                this.Close();
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Evento: mnuSalir_Click
         * Descripción: Cierra la aplicación completamente.
         -----------------------------------------------------------------------------------------------------*/
        private void MenuSalirAplicacion_Click(object sender, EventArgs e)
        {
            /* Pide confirmación antes de salir */
            DialogResult resultado = DialogHelper.MostrarConfirmacion(
                "¿Está seguro que desea salir del sistema?",
                "Salir");

            /* Solo si el usuario confirma se cierra la sesión, se cierran los formularios hijos y se sale.
               Si responde que No, no se toca nada y sigue trabajando con su sesión abierta. */
            if (resultado == DialogResult.Yes)
            {
                /* Cierra la sesión del usuario */
                AuthService authService = new AuthService();
                authService.Logout();

                /* Cierra todos los formularios hijos */
                foreach (Form f in this.MdiChildren)
                    f.Close();

                /* Cierra la aplicación. */
                Application.Exit();
            }
        }
    }
}
