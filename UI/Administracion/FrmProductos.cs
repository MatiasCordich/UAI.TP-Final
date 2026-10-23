using BLL;
using DAL.DAO;
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
using UI.Utils;

namespace UI.Administracion
{
    /* -----------------------------------------------------------------------------------------------------
     * Formulario: FrmProductos
     * Descripción: Formulario de gestión de productos del sistema.
     *              Permite listar, buscar, registrar, modificar, activar y desactivar productos.
     -----------------------------------------------------------------------------------------------------*/
    public partial class FrmProductos : Form
    {
        /* Instancia del manager de productos */
        private ProductoManager productoManager = new ProductoManager();

        /* Instancia del DAO de proveedores para cargar el combo */
        private ProveedorDAO proveedorDAO = new ProveedorDAO();

        /* Guarda el producto seleccionado en el grid */
        private Producto productoSeleccionado = null;

        /* Indica si se está editando o creando un producto */
        private bool modoEdicion = false;

        public FrmProductos()
        {
            InitializeComponent();
        }

        private void FrmProductos_Load(object sender, EventArgs e)
        {
            /* Configura las propiedades del DataGridView */
            ConfigurarGrilla();

            /* Carga el combo de estado */
            CargarEstados();

            /* Carga el combo de proveedores */
            CargarProveedores();

            /* Carga todos los productos sin filtros al inicio */
            CargarProductos();

            /* Deshabilita el panel de edición hasta que se seleccione una acción */
            HabilitarPanel(false);
        }

        /* -----------------------------------------------------------------------------------------------------
         * Evento: dgvProductos_SelectionChanged
         * Descripción: Se ejecuta al seleccionar una fila en la grilla.
         -----------------------------------------------------------------------------------------------------*/
        private void dgvProductos_SelectionChanged(object sender, EventArgs e)
        {
            /* Verifica que haya una fila seleccionada */
            if (dgvProductos.SelectedRows.Count == 0)
                return;

            try
            {
                /* Obtiene el ID del producto seleccionado */
                int id = Convert.ToInt32(dgvProductos.SelectedRows[0].Cells["Id"].Value);

                /* Obtiene el producto completo */
                productoSeleccionado = productoManager.ObtenerProducto(id);
            }
            catch (Exception ex)
            {
                DialogHelper.MostrarError("Error al seleccionar el producto: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Evento: btnBuscar_Click
         * Descripción: Aplica los filtros y recarga la grilla.
         -----------------------------------------------------------------------------------------------------*/
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarProductos();
        }

        /* -----------------------------------------------------------------------------------------------------
         * Evento: btnLimpiar_Click
         * Descripción: Limpia los filtros y recarga todos los productos.
         -----------------------------------------------------------------------------------------------------*/
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            /* Limpia el campo de texto */
            txtFiltroModelo.Clear();

            /* Resetea el combo de estado */
            if (cmbFiltroEstado.Items.Count > 0)
                cmbFiltroEstado.SelectedIndex = 0;

            /* Recarga todos los productos sin filtros */
            CargarProductos();
        }

        /* -----------------------------------------------------------------------------------------------------
         * Evento: btnNuevo_Click
         * Descripción: Prepara el formulario para registrar un nuevo producto.
         -----------------------------------------------------------------------------------------------------*/
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            /* Limpia el panel y lo habilita */
            LimpiarPanel();
            HabilitarPanel(true);

            /* Indica que se está creando un nuevo producto */
            modoEdicion = false;

            /* Pone el foco en el primer campo */
            txtModelo.Focus();
        }

        /* -----------------------------------------------------------------------------------------------------
         * Evento: btnModificar_Click
         * Descripción: Carga los datos del producto seleccionado para modificarlos.
         -----------------------------------------------------------------------------------------------------*/
        private void btnModificar_Click(object sender, EventArgs e)
        {
            /* Verifica que haya un producto seleccionado */
            if (productoSeleccionado == null)
            {
                DialogHelper.MostrarAviso("Debe seleccionar un producto para modificar.");
                return;
            }

            /* Carga los datos del producto en el panel */
            txtModelo.Text = productoSeleccionado.Modelo;
            txtColor.Text = productoSeleccionado.Color;
            txtTalle.Text = productoSeleccionado.Talle.ToString();
            txtPrecio.Text = productoSeleccionado.Precio.ToString();
            txtStock.Text = productoSeleccionado.Stock.ToString();
            txtStockMinimo.Text = productoSeleccionado.StockMinimo.ToString();

            /* Selecciona el proveedor del producto en el combo */
            foreach (Proveedor p in cmbProveedor.Items)
            {
                if (p.Id == productoSeleccionado.IdProveedor)
                {
                    cmbProveedor.SelectedItem = p;
                    break;
                }
            }

            /* Habilita el panel e indica modo edición */
            HabilitarPanel(true);
            modoEdicion = true;
            txtModelo.Focus();
        }

        /* -----------------------------------------------------------------------------------------------------
         * Evento: btnGuardar_Click
         * Descripción: Guarda el producto nuevo o modificado según el modo.
         -----------------------------------------------------------------------------------------------------*/
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                /* Valida que los campos numéricos tengan valores válidos */
                if (!int.TryParse(txtTalle.Text, out int talle))
                {
                    DialogHelper.MostrarAviso("El talle debe ser un número entero.");
                    txtTalle.Focus();
                    return;
                }

                if (!decimal.TryParse(txtPrecio.Text, out decimal precio))
                {
                    DialogHelper.MostrarAviso("El precio debe ser un número válido.");
                    txtPrecio.Focus();
                    return;
                }

                if (!int.TryParse(txtStock.Text, out int stock))
                {
                    DialogHelper.MostrarAviso("El stock debe ser un número entero.");
                    txtStock.Focus();
                    return;
                }

                if (!int.TryParse(txtStockMinimo.Text, out int stockMinimo))
                {
                    DialogHelper.MostrarAviso("El stock mínimo debe ser un número entero.");
                    txtStockMinimo.Focus();
                    return;
                }

                /* Obtiene el proveedor seleccionado */
                Proveedor proveedorSeleccionado = (Proveedor)cmbProveedor.SelectedItem;

                if (modoEdicion)
                {
                    /* Modo edición: actualiza los datos del producto existente */
                    productoSeleccionado.Modelo = txtModelo.Text.Trim();
                    productoSeleccionado.Color = txtColor.Text.Trim();
                    productoSeleccionado.Talle = talle;
                    productoSeleccionado.Precio = precio;
                    productoSeleccionado.Stock = stock;
                    productoSeleccionado.StockMinimo = stockMinimo;
                    productoSeleccionado.IdProveedor = proveedorSeleccionado.Id;

                    /* Llama al manager para modificar */
                    productoManager.ModificarProducto(productoSeleccionado);

                    DialogHelper.MostrarExito("Producto modificado correctamente.");
                }
                else
                {
                    /* Modo creación: crea un nuevo producto */
                    Producto nuevoProducto = new Producto
                    {
                        Modelo = txtModelo.Text.Trim(),
                        Color = txtColor.Text.Trim(),
                        Talle = talle,
                        Precio = precio,
                        Stock = stock,
                        StockMinimo = stockMinimo,
                        IdProveedor = proveedorSeleccionado.Id
                    };

                    /* Llama al manager para registrar */
                    productoManager.RegistrarProducto(nuevoProducto);

                    DialogHelper.MostrarExito("Producto registrado correctamente.");
                }

                /* Recarga la lista y limpia el panel */
                CargarProductos();
                LimpiarPanel();
                HabilitarPanel(false);
            }
            catch (Exception ex)
            {
                DialogHelper.MostrarError(ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Evento: btnDesactivar_Click
         * Descripción: Desactiva el producto seleccionado (baja lógica).
         -----------------------------------------------------------------------------------------------------*/
        private void btnDesactivar_Click(object sender, EventArgs e)
        {
            /* Verifica que haya un producto seleccionado */
            if (productoSeleccionado == null)
            {
                DialogHelper.MostrarAviso("Debe seleccionar un producto para desactivar.");
                return;
            }

            /* Pide confirmación antes de desactivar */
            DialogResult resultado = DialogHelper.MostrarConfirmacion(
                $"¿Está seguro que desea desactivar el producto {productoSeleccionado.Modelo}?",
                "Confirmar desactivación");

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    /* Desactiva el producto */
                    productoManager.DesactivarProducto(productoSeleccionado.Id);

                    DialogHelper.MostrarExito("Producto desactivado correctamente.");

                    /* Recarga la lista y limpia el panel */
                    CargarProductos();
                    LimpiarPanel();
                }
                catch (Exception ex)
                {
                    DialogHelper.MostrarError(ex.Message);
                }
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Evento: btnActivar_Click
         * Descripción: Reactiva un producto desactivado.
         -----------------------------------------------------------------------------------------------------*/
        private void btnActivar_Click(object sender, EventArgs e)
        {
            /* Verifica que haya un producto seleccionado */
            if (productoSeleccionado == null)
            {
                DialogHelper.MostrarAviso("Debe seleccionar un producto para activar.");
                return;
            }

            /* Pide confirmación antes de activar */
            DialogResult resultado = DialogHelper.MostrarConfirmacion(
                $"¿Está seguro que desea activar el producto {productoSeleccionado.Modelo}?",
                "Confirmar activación");

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    /* Activa el producto */
                    productoManager.ActivarProducto(productoSeleccionado.Id);

                    DialogHelper.MostrarExito("Producto activado correctamente.");

                    /* Recarga la lista y limpia el panel */
                    CargarProductos();
                    LimpiarPanel();
                }
                catch (Exception ex)
                {
                    DialogHelper.MostrarError(ex.Message);
                }
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Evento: btnCancelar_Click
         * Descripción: Cancela la operación actual y limpia el panel.
         -----------------------------------------------------------------------------------------------------*/
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            /* Limpia el panel y lo deshabilita */
            LimpiarPanel();
            HabilitarPanel(false);
        }

        /* -----------------------------------------------------------------------------------------------------
        * MÉTODOS PRIVADOS
        -----------------------------------------------------------------------------------------------------*/

        private void ConfigurarGrilla()
        {
            dgvProductos.ReadOnly = true;
            dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductos.RowHeadersVisible = false;
            dgvProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProductos.AllowUserToAddRows = false;
        }

        private void CargarEstados()
        {
            cmbFiltroEstado.Items.Clear();
            cmbFiltroEstado.Items.Add("Todos");
            cmbFiltroEstado.Items.Add("Activo");
            cmbFiltroEstado.Items.Add("Inactivo");
            cmbFiltroEstado.SelectedIndex = 0;
        }

        private void CargarProveedores()
        {
            try
            {
                /* Obtiene todos los proveedores activos */
                List<Proveedor> proveedores = proveedorDAO.GetAll();

                /* Configura el ComboBox para mostrar el nombre y guardar el ID */
                cmbProveedor.DisplayMember = "Nombre";
                cmbProveedor.ValueMember = "Id";
                cmbProveedor.DataSource = proveedores;
            }
            catch (Exception ex)
            {
                DialogHelper.MostrarError("Error al cargar los proveedores: " + ex.Message);
            }
        }

        private void CargarProductos()
        {
            try
            {
                /* Obtiene el filtro de modelo */
                string modelo = string.IsNullOrEmpty(txtFiltroModelo.Text)
                                ? null
                                : txtFiltroModelo.Text.Trim();

                /* Obtiene el filtro de estado */
                bool? activo = null;
                if (cmbFiltroEstado.SelectedItem?.ToString() == "Activo")
                    activo = true;
                else if (cmbFiltroEstado.SelectedItem?.ToString() == "Inactivo")
                    activo = false;

                /* Obtiene los productos con los filtros aplicados */
                List<Producto> productos = productoManager.ListarProductosOFiltrados(modelo, activo);

                /* Limpia la grilla */
                dgvProductos.Rows.Clear();
                dgvProductos.Columns.Clear();

                /* Define las columnas de la grilla */
                dgvProductos.Columns.Add("Id", "ID");
                dgvProductos.Columns.Add("Modelo", "Modelo");
                dgvProductos.Columns.Add("Color", "Color");
                dgvProductos.Columns.Add("Talle", "Talle");
                dgvProductos.Columns.Add("Precio", "Precio");
                dgvProductos.Columns.Add("Stock", "Stock");
                dgvProductos.Columns.Add("StockMinimo", "Stock mín.");
                dgvProductos.Columns.Add("Estado", "Estado");

                /* Agrega cada producto como fila en la grilla */
                foreach (Producto p in productos)
                {
                    int index = dgvProductos.Rows.Add(
                        p.Id,
                        p.Modelo,
                        p.Color,
                        p.Talle,
                        p.Precio.ToString("C"),
                        p.Stock,
                        p.StockMinimo,
                        p.Activo ? "Activo" : "Inactivo"
                    );

                    /* Colorea la celda de Estado según el valor */
                    if (p.Activo)
                    {
                        dgvProductos.Rows[index].Cells["Estado"].Style.BackColor = Color.LightGreen;
                        dgvProductos.Rows[index].Cells["Estado"].Style.ForeColor = Color.DarkGreen;
                    }
                    else
                    {
                        dgvProductos.Rows[index].Cells["Estado"].Style.BackColor = Color.LightCoral;
                        dgvProductos.Rows[index].Cells["Estado"].Style.ForeColor = Color.DarkRed;
                    }

                    /* Colorea la fila si el stock llegó al mínimo */
                    if (p.Activo && p.Stock <= p.StockMinimo)
                    {
                        dgvProductos.Rows[index].Cells["Stock"].Style.BackColor = Color.LightYellow;
                        dgvProductos.Rows[index].Cells["Stock"].Style.ForeColor = Color.DarkGoldenrod;
                    }
                }
            }
            catch (Exception ex)
            {
                DialogHelper.MostrarError("Error al cargar los productos: " + ex.Message);
            }
        }

        private void HabilitarPanel(bool habilitar)
        {
            txtModelo.Enabled = habilitar;
            txtColor.Enabled = habilitar;
            txtTalle.Enabled = habilitar;
            txtPrecio.Enabled = habilitar;
            txtStock.Enabled = habilitar;
            txtStockMinimo.Enabled = habilitar;
            cmbProveedor.Enabled = habilitar;
            btnGuardar.Enabled = habilitar;
            btnCancelar.Enabled = habilitar;
        }

        private void LimpiarPanel()
        {
            txtModelo.Clear();
            txtColor.Clear();
            txtTalle.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
            txtStockMinimo.Clear();

            if (cmbProveedor.Items.Count > 0)
                cmbProveedor.SelectedIndex = 0;

            productoSeleccionado = null;
            modoEdicion = false;
        }
    }

}
