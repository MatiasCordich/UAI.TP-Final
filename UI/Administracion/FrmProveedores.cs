using BLL;
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
 * Formulario: FrmProveedores
 * Descripción: Formulario de gestión de proveedores del sistema.
 *              Permite listar, buscar, registrar, modificar, activar y desactivar proveedores.
 *              El CUIT se guarda encriptado, pero el ProveedorManager lo entrega desencriptado.
 *              La deuda total se muestra en la grilla pero no se edita desde acá: es la cuenta
 *              corriente del proveedor y la modifican los pedidos y los pagos.
 -----------------------------------------------------------------------------------------------------*/
    public partial class FrmProveedores : Form
    {

        /* Instancia del manager de proveedores */
        private ProveedorManager proveedorManager = new ProveedorManager();

        /* Proveedor que está seleccionado en la grilla */
        private Proveedor proveedorSeleccionado = null;

        /* Proveedor que se está modificando en el panel de edición.
           Es independiente de la selección de la grilla: si se vuelve a buscar mientras se edita,
           el panel sigue trabajando sobre el proveedor correcto. */
        private Proveedor proveedorEnEdicion = null;

        /* Indica si se está editando un proveedor existente (true) o creando uno nuevo (false) */
        private bool modoEdicion = false;

        public FrmProveedores()
        {
            InitializeComponent();
        }

         /* -----------------------------------------------------------------------------------------------------
         * Evento: FrmProveedores_Load
         * Descripción: Se ejecuta al cargar el formulario.
         -----------------------------------------------------------------------------------------------------*/
        private void FrmProveedores_Load(object sender, EventArgs e)
        {
            /* Configura las propiedades del DataGridView */
            ConfigurarGrilla();

            /* Carga el combo de estado (debe ir antes de cargar la grilla, que lo consulta) */
            CargarEstados();

            /* Carga todos los proveedores sin filtros al inicio */
            CargarProveedores();

            /* Deshabilita el panel de edición hasta que se seleccione una acción */
            HabilitarPanel(false);
        }

        /* -----------------------------------------------------------------------------------------------------
         * Evento: dgvProveedores_SelectionChanged
         * Descripción: Se ejecuta al seleccionar una fila en la grilla.
         -----------------------------------------------------------------------------------------------------*/
        private void dgvProveedores_SelectionChanged(object sender, EventArgs e)
        {
            /* Si no hay ninguna fila seleccionada, no hay proveedor seleccionado */
            if (dgvProveedores.SelectedRows.Count == 0)
            {
                proveedorSeleccionado = null;
                return;
            }

            try
            {
                /* Obtiene el ID del proveedor seleccionado */
                int id = Convert.ToInt32(dgvProveedores.SelectedRows[0].Cells["Id"].Value);

                /* Obtiene el proveedor completo, con el CUIT desencriptado */
                proveedorSeleccionado = proveedorManager.ObtenerProveedor(id);
            }
            catch (Exception ex)
            {
                DialogHelper.MostrarError("Error al seleccionar el proveedor: " + ex.Message);
            }
        }

         /* -----------------------------------------------------------------------------------------------------
         * Evento: btnBuscar_Click
         * Descripción: Aplica los filtros y recarga la grilla.
         -----------------------------------------------------------------------------------------------------*/
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarProveedores();
        }

         /* -----------------------------------------------------------------------------------------------------
         * Evento: btnLimpiar_Click
         * Descripción: Limpia los filtros y recarga todos los proveedores.
         -----------------------------------------------------------------------------------------------------*/
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            /* Limpia el campo de texto */
            txtFiltroNombre.Clear();

            /* Resetea el combo de estado solo si tiene items */
            if (cmbFiltroEstado.Items.Count > 0)
                cmbFiltroEstado.SelectedIndex = 0;

            /* Recarga todos los proveedores sin filtros */
            CargarProveedores();
        }

         /* -----------------------------------------------------------------------------------------------------
         * Evento: btnNuevo_Click
         * Descripción: Prepara el formulario para registrar un nuevo proveedor.
         -----------------------------------------------------------------------------------------------------*/
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            /* Limpia el panel y lo habilita */
            LimpiarPanel();
            HabilitarPanel(true);

            /* Indica que se está creando un nuevo proveedor */
            modoEdicion = false;

            /* Pone el foco en el primer campo */
            txtNombre.Focus();
        }

         /* -----------------------------------------------------------------------------------------------------
         * Evento: btnModificar_Click
         * Descripción: Carga los datos del proveedor seleccionado para modificarlos.
         -----------------------------------------------------------------------------------------------------*/
        private void btnModificar_Click(object sender, EventArgs e)
        {
            /* Verifica que haya un proveedor seleccionado */
            if (proveedorSeleccionado == null)
            {
                DialogHelper.MostrarAviso("Debe seleccionar un proveedor para modificar.");
                return;
            }

            /* Un proveedor desactivado no se puede modificar.
               Se avisa antes de abrir el panel para no hacer escribir datos que luego se rechazarían */
            if (!proveedorSeleccionado.Activo)
            {
                DialogHelper.MostrarAviso("Un proveedor desactivado no se puede modificar. Actívelo primero.");
                return;
            }

            /* Guarda el proveedor que se va a editar */
            proveedorEnEdicion = proveedorSeleccionado;

            /* Carga los datos del proveedor en el panel */
            txtNombre.Text = proveedorEnEdicion.Nombre;
            txtCuit.Text = proveedorEnEdicion.Cuit;
            txtTelefono.Text = proveedorEnEdicion.Telefono;
            txtEmail.Text = proveedorEnEdicion.Email;

            /* Habilita el panel e indica modo edición */
            HabilitarPanel(true);
            modoEdicion = true;
            txtNombre.Focus();
        }

         /* -----------------------------------------------------------------------------------------------------
         * Evento: btnActivar_Click
         * Descripción: Reactiva un proveedor desactivado.
         -----------------------------------------------------------------------------------------------------*/
        private void btnActivar_Click(object sender, EventArgs e)
        {
            /* Verifica que haya un proveedor seleccionado */
            if (proveedorSeleccionado == null)
            {
                DialogHelper.MostrarAviso("Debe seleccionar un proveedor para activar.");
                return;
            }

            /* Pide confirmación antes de activar */
            DialogResult resultado = DialogHelper.MostrarConfirmacion(
                $"¿Está seguro que desea activar al proveedor {proveedorSeleccionado.Nombre}?",
                "Confirmar activación");

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    /* Activa el proveedor */
                    proveedorManager.ActivarProveedor(proveedorSeleccionado.Id);

                    DialogHelper.MostrarExito("Proveedor activado correctamente.");

                    /* Recarga la lista y cierra el panel */
                    CargarProveedores();
                    CerrarPanel();
                }
                catch (Exception ex)
                {
                    DialogHelper.MostrarError(ex.Message);
                }
            }
        }

         /* -----------------------------------------------------------------------------------------------------
         * Evento: btnDesactivar_Click
         * Descripción: Desactiva el proveedor seleccionado (baja lógica).
         *              El manager rechaza la baja si el proveedor tiene deuda pendiente.
         -----------------------------------------------------------------------------------------------------*/
        private void btnDesactivar_Click(object sender, EventArgs e)
        {
            /* Verifica que haya un proveedor seleccionado */
            if (proveedorSeleccionado == null)
            {
                DialogHelper.MostrarAviso("Debe seleccionar un proveedor para desactivar.");
                return;
            }

            /* Pide confirmación antes de desactivar */
            DialogResult resultado = DialogHelper.MostrarConfirmacion(
                $"¿Está seguro que desea desactivar al proveedor {proveedorSeleccionado.Nombre}?",
                "Confirmar desactivación");

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    /* Desactiva el proveedor */
                    proveedorManager.DesactivarProveedor(proveedorSeleccionado.Id);

                    DialogHelper.MostrarExito("Proveedor desactivado correctamente.");

                    /* Recarga la lista y cierra el panel */
                    CargarProveedores();
                    CerrarPanel();
                }
                catch (Exception ex)
                {
                    DialogHelper.MostrarError(ex.Message);
                }
            }
        }

         /* -----------------------------------------------------------------------------------------------------
         * Evento: btnGuardar_Click
         * Descripción: Guarda el proveedor nuevo o modificado según el modo.
         *              Esta pantalla solo envía los datos: las validaciones las hace el ProveedorManager.
         -----------------------------------------------------------------------------------------------------*/
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (modoEdicion)
                {
                    /* Modo edición: actualiza los datos del proveedor que se está editando.
                       La deuda total no se toca: el manager conserva la que ya tiene guardada */
                    proveedorEnEdicion.Nombre = txtNombre.Text.Trim();
                    proveedorEnEdicion.Cuit = txtCuit.Text.Trim();
                    proveedorEnEdicion.Telefono = txtTelefono.Text.Trim();
                    proveedorEnEdicion.Email = txtEmail.Text.Trim();

                    /* Llama al manager para modificar */
                    proveedorManager.ModificarProveedor(proveedorEnEdicion);

                    DialogHelper.MostrarExito("Proveedor modificado correctamente.");
                }
                else
                {
                    /* Modo creación: crea un nuevo proveedor.
                       El manager lo deja activo y con deuda en cero */
                    Proveedor nuevoProveedor = new Proveedor
                    {
                        Nombre = txtNombre.Text.Trim(),
                        Cuit = txtCuit.Text.Trim(),
                        Telefono = txtTelefono.Text.Trim(),
                        Email = txtEmail.Text.Trim()
                    };

                    /* Llama al manager para registrar */
                    proveedorManager.RegistrarProveedor(nuevoProveedor);

                    DialogHelper.MostrarExito("Proveedor registrado correctamente.");
                }

                /* Recarga la lista y cierra el panel */
                CargarProveedores();
                CerrarPanel();
            }
            catch (Exception ex)
            {
                DialogHelper.MostrarError(ex.Message);
            }
        }

         /* -----------------------------------------------------------------------------------------------------
         * Evento: btnCancelar_Click
         * Descripción: Cancela la operación actual y cierra el panel de edición.
         -----------------------------------------------------------------------------------------------------*/
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            CerrarPanel();
        }

         /* -----------------------------------------------------------------------------------------------------
         * MÉTODOS PRIVADOS
         -----------------------------------------------------------------------------------------------------*/
 
        /* -----------------------------------------------------------------------------------------------------
         * Función: ConfigurarGrilla
         * Descripción: Configura las propiedades del DataGridView.
         -----------------------------------------------------------------------------------------------------*/
        private void ConfigurarGrilla()
        {
            /* No permite edición directa en la grilla */
            dgvProveedores.ReadOnly = true;
 
            /* Selecciona la fila completa al hacer clic */
            dgvProveedores.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
 
            /* No muestra la columna de encabezado de fila */
            dgvProveedores.RowHeadersVisible = false;
 
            /* Ajusta las columnas al ancho del control */
            dgvProveedores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
 
            /* No permite agregar filas nuevas desde la grilla (evita la fila vacía del final) */
            dgvProveedores.AllowUserToAddRows = false;
        }
 
        /* -----------------------------------------------------------------------------------------------------
         * Función: CargarEstados
         * Descripción: Carga las opciones de estado en el combo de filtro.
         -----------------------------------------------------------------------------------------------------*/
        private void CargarEstados()
        {
            cmbFiltroEstado.Items.Clear();
            cmbFiltroEstado.Items.Add("Todos");
            cmbFiltroEstado.Items.Add("Activo");
            cmbFiltroEstado.Items.Add("Inactivo");
 
            /* Selecciona Todos por defecto */
            cmbFiltroEstado.SelectedIndex = 0;
        }
 
        /* -----------------------------------------------------------------------------------------------------
         * Función: CargarProveedores
         * Descripción: Carga la lista de proveedores aplicando los filtros actuales.
         -----------------------------------------------------------------------------------------------------*/
        private void CargarProveedores()
        {
            try
            {
                /* Obtiene el filtro de nombre (null si el campo está vacío) */
                string nombre = string.IsNullOrEmpty(txtFiltroNombre.Text)
                                ? null
                                : txtFiltroNombre.Text.Trim();
 
                /* Obtiene el filtro de estado (null si es Todos) */
                bool? activo = null;
                if (cmbFiltroEstado.SelectedItem?.ToString() == "Activo")
                    activo = true;
                else if (cmbFiltroEstado.SelectedItem?.ToString() == "Inactivo")
                    activo = false;
 
                /* Obtiene los proveedores con los filtros aplicados, con el CUIT desencriptado */
                List<Proveedor> proveedores = proveedorManager.ListarProveedoresOFiltrados(nombre, activo);
 
                /* Limpia la grilla */
                dgvProveedores.Rows.Clear();
                dgvProveedores.Columns.Clear();
 
                /* Define las columnas de la grilla */
                dgvProveedores.Columns.Add("Id",       "ID");
                dgvProveedores.Columns.Add("Nombre",   "Nombre");
                dgvProveedores.Columns.Add("Cuit",     "CUIT");
                dgvProveedores.Columns.Add("Telefono", "Teléfono");
                dgvProveedores.Columns.Add("Email",    "Email");
                dgvProveedores.Columns.Add("Deuda",    "Deuda");
                dgvProveedores.Columns.Add("Estado",   "Estado");
 
                /* Agrega cada proveedor como fila en la grilla */
                foreach (Proveedor p in proveedores)
                {
                    int index = dgvProveedores.Rows.Add(
                        p.Id,
                        p.Nombre,
                        p.Cuit,
                        p.Telefono,
                        p.Email,
                        p.DeudaTotal.ToString("C"),
                        p.Activo ? "Activo" : "Inactivo"
                    );
 
                    /* Colorea la celda de Estado según el valor */
                    if (p.Activo)
                    {
                        dgvProveedores.Rows[index].Cells["Estado"].Style.BackColor = Color.LightGreen;
                        dgvProveedores.Rows[index].Cells["Estado"].Style.ForeColor = Color.DarkGreen;
                    }
                    else
                    {
                        dgvProveedores.Rows[index].Cells["Estado"].Style.BackColor = Color.LightCoral;
                        dgvProveedores.Rows[index].Cells["Estado"].Style.ForeColor = Color.DarkRed;
                    }
 
                    /* Resalta la deuda cuando es mayor a cero: ese proveedor no se puede desactivar */
                    if (p.DeudaTotal > 0)
                    {
                        dgvProveedores.Rows[index].Cells["Deuda"].Style.BackColor = Color.LightYellow;
                        dgvProveedores.Rows[index].Cells["Deuda"].Style.ForeColor = Color.DarkGoldenrod;
                    }
                }
 
                /* Deja la grilla sin fila marcada, así lo que se ve coincide con el proveedor seleccionado */
                dgvProveedores.ClearSelection();
            }
            catch (Exception ex)
            {
                DialogHelper.MostrarError("Error al cargar los proveedores: " + ex.Message);
            }
        }
 
        /* -----------------------------------------------------------------------------------------------------
         * Función: HabilitarPanel
         * Descripción: Habilita o deshabilita el panel de edición de datos.
         -----------------------------------------------------------------------------------------------------*/
        private void HabilitarPanel(bool habilitar)
        {
            txtNombre.Enabled   = habilitar;
            txtCuit.Enabled     = habilitar;
            txtTelefono.Enabled = habilitar;
            txtEmail.Enabled    = habilitar;
            btnGuardar.Enabled  = habilitar;
            btnCancelar.Enabled = habilitar;
        }
 
        /* -----------------------------------------------------------------------------------------------------
         * Función: LimpiarPanel
         * Descripción: Limpia los campos del panel de edición y el proveedor que se estaba editando.
         -----------------------------------------------------------------------------------------------------*/
        private void LimpiarPanel()
        {
            txtNombre.Clear();
            txtCuit.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
 
            proveedorEnEdicion = null;
            modoEdicion = false;
        }
 
        /* -----------------------------------------------------------------------------------------------------
         * Función: CerrarPanel
         * Descripción: Limpia y deshabilita el panel de edición. Se usa al terminar o cancelar una operación.
         -----------------------------------------------------------------------------------------------------*/
        private void CerrarPanel()
        {
            LimpiarPanel();
            HabilitarPanel(false);
        }
    }
}
