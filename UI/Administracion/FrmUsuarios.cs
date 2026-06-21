using BLL;
using BLL.SECURITY;
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

namespace UI.Administracion
{
    /* -----------------------------------------------------------------------------------------------------
     * Formulario: FrmUsuarios
     * Descripción: Formulario de gestión de usuarios del sistema.
     *              Permite listar, registrar, modificar, activar y desactivar usuarios.
     -----------------------------------------------------------------------------------------------------*/
    public partial class FrmUsuarios : Form
    {
        /* Instancia del manager de usuarios */
        private UsuarioManager usuarioManager = new UsuarioManager();

        /* Instancia del DAO de roles para cargar el combo */
        private RolDAO rolDAO = new RolDAO();

        /* Guarda el usuario seleccionado en el grid */
        private Usuario usuarioSeleccionado = null;

        /* Indica si se está editando o creando un usuario */
        private bool modoEdicion = false;
        public FrmUsuarios()
        {
            /* Inicializa los componentes del formulario */
            InitializeComponent();
        }

        /* -----------------------------------------------------------------------------------------------------
         * Evento: FrmUsuarios_Load
         * Descripción: Se ejecuta al cargar el formulario.
         -----------------------------------------------------------------------------------------------------*/
        private void FrmUsuarios_Load(object sender, EventArgs e)
        {
            /* Configura las columnas del DataGridView */
            ConfigurarGrilla();

            /* Carga los roles disponibles en el ComboBox */
            CargarRoles();

            /* Carga el combo de estado con las opciones disponibles */
            CargarEstados();

            /* Carga la lista de usuarios */
            CargarUsuarios();

            /* Deshabilita el panel de edición hasta que se seleccione una acción */
            HabilitarPanel(false);
        }

        /* -----------------------------------------------------------------------------------------------------
         * Evento: txtNombre_Leave
         * Descripción: Se ejecuta al salir del campo Nombre.
         *              Regenera el nombre de usuario automáticamente.
         -----------------------------------------------------------------------------------------------------*/
        private void txtNombre_Leave(object sender, EventArgs e)
        {
            /* Regenera el nombre de usuario al salir del campo nombre */
            ActualizarNombreUsuario();
        }

         /* -----------------------------------------------------------------------------------------------------
         * Evento: txtNombre_Leave
         * Descripción: Se ejecuta al salir del campo Nombre.
         *              Regenera el nombre de usuario automáticamente.
         -----------------------------------------------------------------------------------------------------*/
        private void txtApellido_Leave(object sender, EventArgs e)
        {
            /* Regenera el nombre de usuario al salir del campo apellido */
            ActualizarNombreUsuario();
        }

        /* -----------------------------------------------------------------------------------------------------
         * Evento: dgvUsuarios_SelectionChanged
         * Descripción: Se ejecuta al seleccionar una fila en la grilla.
         -----------------------------------------------------------------------------------------------------*/
        private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            /* Verifica que haya una fila seleccionada */
            if (dgvUsuarios.SelectedRows.Count == 0)
                return;

            try
            {
                /* Obtiene el ID del usuario seleccionado */
                int id = Convert.ToInt32(dgvUsuarios.SelectedRows[0].Cells["Id"].Value);

                /* Obtiene el usuario completo */
                usuarioSeleccionado = usuarioManager.ObtenerUsuario(id);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al seleccionar el usuario: " + ex.Message,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void btnInsertar_Click(object sender, EventArgs e)
        {
            /* Limpia el panel y lo habilita */
            LimpiarPanel();
            HabilitarPanel(true);

            /* Indica que se está creando un nuevo usuario */
            modoEdicion = false;

            /* Pone el foco en el primer campo */
            txtNombre.Focus();
        }

        /* -----------------------------------------------------------------------------------------------------
         * Evento: btnModificar_Click
         * Descripción: Carga los datos del usuario seleccionado para modificarlos.
         -----------------------------------------------------------------------------------------------------*/
        private void btnModificar_Click(object sender, EventArgs e)
        {
            /* Verifica que haya un usuario seleccionado */
            if (usuarioSeleccionado == null)
            {
                MessageBox.Show("Debe seleccionar un usuario para modificar.",
                                "Aviso",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            /* Carga los datos del usuario en el panel */
            txtNombre.Text = usuarioSeleccionado.Nombre;
            txtApellido.Text = usuarioSeleccionado.Apellido;
            txtUsuario.Text = usuarioSeleccionado.NombreUsuario;

            /* Selecciona el rol del usuario en el combo */
            foreach (Rol r in cmbRol.Items)
            {
                if (r.Id == usuarioSeleccionado.IdRol)
                {
                    cmbRol.SelectedItem = r;
                    break;
                }
            }

            /* Habilita el panel */
            HabilitarPanel(true);

            /* Indica que se está editando un usuario existente */
            modoEdicion = true;

            /* Pone el foco en el primer campo */
            txtNombre.Focus();
        }

        /* -----------------------------------------------------------------------------------------------------
         * Evento: btnDesactivar_Click
         * Descripción: Desactiva el usuario seleccionado (baja lógica).
         -----------------------------------------------------------------------------------------------------*/
        private void btnDesactivar_Click(object sender, EventArgs e)
        {
            /* Verifica que haya un usuario seleccionado */
            if (usuarioSeleccionado == null)
            {
                MessageBox.Show("Debe seleccionar un usuario para desactivar.",
                                "Aviso",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            /* Pide confirmación antes de desactivar */
            DialogResult resultado = MessageBox.Show(
                $"¿Está seguro que desea desactivar al usuario {usuarioSeleccionado.NombreUsuario}?",
                "Confirmar desactivación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    /* Desactiva el usuario pasando también el ID del usuario actual */
                    usuarioManager.DesactivarUsuario(usuarioSeleccionado.Id,
                                                     AuthService.UsuarioActual.Id);

                    MessageBox.Show("Usuario desactivado correctamente.",
                                    "Éxito",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                    /* Recarga la lista y limpia el panel */
                    CargarUsuarios();
                    LimpiarPanel();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message,
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Evento: btnActivar_Click
         * Descripción: Reactiva un usuario desactivado.
         -----------------------------------------------------------------------------------------------------*/
        private void btnActivar_Click(object sender, EventArgs e)
        {
            /* Verifica que haya un usuario seleccionado */
            if (usuarioSeleccionado == null)
            {
                MessageBox.Show("Debe seleccionar un usuario para activar.",
                                "Aviso",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            /* Pide confirmación antes de activar */
            DialogResult resultado = MessageBox.Show(
                $"¿Está seguro que desea activar al usuario {usuarioSeleccionado.NombreUsuario}?",
                "Confirmar activación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    /* Activa el usuario */
                    usuarioManager.ActivarUsuario(usuarioSeleccionado.Id);

                    MessageBox.Show("Usuario activado correctamente.",
                                    "Éxito",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                    /* Recarga la lista y limpia el panel */
                    CargarUsuarios();
                    LimpiarPanel();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message,
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Evento: btnGuardar_Click
         * Descripción: Guarda el usuario nuevo o modificado según el modo.
         -----------------------------------------------------------------------------------------------------*/
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                /* Obtiene el rol seleccionado */
                Rol rolSeleccionado = (Rol)cmbRol.SelectedItem;

                if (modoEdicion)
                {
                    /* Modo edición: actualiza los datos del usuario existente */
                    usuarioSeleccionado.Nombre = txtNombre.Text.Trim();
                    usuarioSeleccionado.Apellido = txtApellido.Text.Trim();
                    usuarioSeleccionado.IdRol = rolSeleccionado.Id;

                    /* Llama al manager para modificar */
                    usuarioManager.ModificarUsuario(usuarioSeleccionado);

                    MessageBox.Show("Usuario modificado correctamente.",
                                    "Éxito",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }
                else
                {
                    /* Modo creación: crea un nuevo usuario */
                    Usuario nuevoUsuario = new Usuario
                    {
                        Nombre = txtNombre.Text.Trim(),
                        Apellido = txtApellido.Text.Trim(),
                        IdRol = rolSeleccionado.Id
                    };

                    /* Registra el usuario y obtiene la contraseña temporal */
                    string contrasenaTemporal = usuarioManager.RegistrarUsuario(nuevoUsuario);

                    /* Muestra las credenciales al administrador */
                    MessageBox.Show(
                        $"Usuario creado correctamente.\n\n" +
                        $"Usuario: {nuevoUsuario.NombreUsuario}\n" +
                        $"Contraseña temporal: {contrasenaTemporal}\n\n" +
                        $"Comunique estas credenciales al usuario.",
                        "Usuario creado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                /* Recarga la lista y limpia el panel */
                CargarUsuarios();
                LimpiarPanel();
                HabilitarPanel(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
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
         * Evento: btnBuscar_Click
         * Descripción: Aplica los filtros y recarga la grilla.
         -----------------------------------------------------------------------------------------------------*/
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            /* Recarga la grilla con los filtros actuales */
            CargarUsuarios();
        }

        /* -----------------------------------------------------------------------------------------------------
        * Evento: btnLimpiarBusqueda_Click
        * Descripción: Limpia los filtros y recarga todos los usuarios.
        -----------------------------------------------------------------------------------------------------*/
        private void btnLimpiarBusqueda_Click(object sender, EventArgs e)
        {
            /* Limpia el campo de texto */
            txtFiltroUsuario.Clear();

            /* Resetea los combos solo si tienen items */
            if (cmbFiltroRol.Items.Count > 0)
                cmbFiltroRol.SelectedIndex = 0;

            if (cmbFiltroEstado.Items.Count > 0)
                cmbFiltroEstado.SelectedIndex = 0;

            /* Recarga todos los usuarios sin filtros */
            CargarUsuarios();
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
            dgvUsuarios.ReadOnly = true;

            /* Selecciona la fila completa al hacer clic */
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            /* No muestra la columna de encabezado de fila */
            dgvUsuarios.RowHeadersVisible = false;

            /* Ajusta las columnas al ancho del control */
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: CargarRoles
         * Descripción: Carga los roles disponibles en el ComboBox.
         -----------------------------------------------------------------------------------------------------*/
        private void CargarRoles()
        {
            try
            {
                /* Obtiene todos los roles del XML */
                List<Rol> roles = rolDAO.GetAll();

                /* Configura el ComboBox de edición */
                cmbRol.DisplayMember = "Nombre";
                cmbRol.ValueMember = "Id";
                cmbRol.DataSource = new List<Rol>(roles);

                /* Agrega la opción "Todos" al comboBox de busqueda filtrada. */
                List<Rol> rolesConTodos = new List<Rol>();
                rolesConTodos.Add(new Rol { Id = 0, Nombre = "Todos" });
                rolesConTodos.AddRange(roles);

                /* Configura el ComboBox de filtro */
                cmbFiltroRol.DisplayMember = "Nombre";
                cmbFiltroRol.ValueMember = "Id";
                cmbFiltroRol.DataSource = rolesConTodos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los roles: " + ex.Message,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

         /* -----------------------------------------------------------------------------------------------------
         * Función: CargarEstados
         * Descripción: Carga las opciones de estado en el combo de filtro.
         -----------------------------------------------------------------------------------------------------*/
        private void CargarEstados()
        {
            /* Agrega las opciones de estado al combo */
            cmbFiltroEstado.Items.Clear();
            cmbFiltroEstado.Items.Add("Todos");
            cmbFiltroEstado.Items.Add("Activo");
            cmbFiltroEstado.Items.Add("Inactivo");
 
            /* Selecciona Todos por defecto */
            cmbFiltroEstado.SelectedIndex = 0;
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: CargarUsuarios
         * Descripción: Carga la lista de usuarios en el DataGridView.
         -----------------------------------------------------------------------------------------------------*/
        private void CargarUsuarios()
        {
            try
            {

                /* Obtiene los valores de los filtros */
                string nombreUsuario = string.IsNullOrEmpty(txtFiltroUsuario.Text)
                                       ? null
                                       : txtFiltroUsuario.Text.Trim();

                /* Obtiene el rol seleccionado (null si es Todos) */
                int? idRol = null;
                if (cmbFiltroRol.SelectedItem is Rol rolSeleccionado && rolSeleccionado.Id != 0)
                    idRol = rolSeleccionado.Id;

                /* Obtiene el estado seleccionado (null si es Todos) */
                bool? activo = null;
                if (cmbFiltroEstado.SelectedItem?.ToString() == "Activo")
                    activo = true;
                else if (cmbFiltroEstado.SelectedItem?.ToString() == "Inactivo")
                    activo = false;

                /* Obtiene todos los usuarios activos */
                List<Usuario> usuarios = usuarioManager.ListarUsuarios(nombreUsuario, idRol, activo);

                /* Limpia la grilla */
                dgvUsuarios.Rows.Clear();
                dgvUsuarios.Columns.Clear();

                /* Define las columnas de la grilla */
                dgvUsuarios.Columns.Add("Id", "ID");
                dgvUsuarios.Columns.Add("NombreUsuario", "Usuario");
                dgvUsuarios.Columns.Add("Nombre", "Nombre");
                dgvUsuarios.Columns.Add("Apellido", "Apellido");
                dgvUsuarios.Columns.Add("Rol", "Rol");
                dgvUsuarios.Columns.Add("Estado", "Estado");

                /* Agrega cada usuario como fila en la grilla */
                foreach (Usuario u in usuarios)
                {
                    dgvUsuarios.Rows.Add(
                        u.Id,
                        u.NombreUsuario,
                        u.Nombre,
                        u.Apellido,
                        u.Rol?.Nombre,
                        u.Activo ? "Activo" : "Inactivo"
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los usuarios: " + ex.Message,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: HabilitarPanel
         * Descripción: Habilita o deshabilita el panel de edición de datos.
         -----------------------------------------------------------------------------------------------------*/
        private void HabilitarPanel(bool habilitar)
        {
            /* Habilita o deshabilita los campos de edición */
            txtNombre.Enabled = habilitar;
            txtApellido.Enabled = habilitar;
            cmbRol.Enabled = habilitar;
            btnGuardar.Enabled = habilitar;
            btnCancelar.Enabled = habilitar;
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: LimpiarPanel
         * Descripción: Limpia los campos del panel de edición.
         -----------------------------------------------------------------------------------------------------*/
        private void LimpiarPanel()
        {
            /* Limpia todos los campos */
            txtNombre.Clear();
            txtApellido.Clear();
            txtUsuario.Clear();

            /* Resetea el combo al primer elemento */
            if (cmbRol.Items.Count > 0)
                cmbRol.SelectedIndex = 0;

            /* Limpia el usuario seleccionado */
            usuarioSeleccionado = null;
            modoEdicion = false;
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: ActualizarNombreUsuario
         * Descripción: Regenera el nombre de usuario automáticamente al escribir nombre o apellido.
         *              Solo actúa en modo creación, no en modo edición.
         -----------------------------------------------------------------------------------------------------*/
        private void ActualizarNombreUsuario()
        {
            /* Solo regenera en modo creación */
            if (modoEdicion)
                return;

            /* Llama al método público de previsualización */
            txtUsuario.Text = usuarioManager.PreVisualizarNombreUsuario(
                txtNombre.Text.Trim(),
                txtApellido.Text.Trim()
            );
        }
    }
}
