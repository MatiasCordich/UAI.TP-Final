using BLL;
using BLL.SECURITY;
using DAL.DAO;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using UI.Utils;

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

        /* Instancia del servicio de autenticación para resetear claves */
        private AuthService authService = new AuthService();

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

            /* Carga los roles disponibles en el ComboBox */
            CargarRoles();

            /* Carga los estados disponibles en el ComboBox de búsqueda para filtrar usuarios. */
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
        * Evento: txtApellido_Leave
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
                DialogHelper.MostrarError("Error al seleccionar el usuario: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
        * Evento: btnInsertar_Click
        * Descripción: Lógica para activar el panel cuando se quiere registrar un nuevo usuario.
        -----------------------------------------------------------------------------------------------------*/
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
                DialogHelper.MostrarAviso("Debe seleccionar un usuario para modificar.");
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
                DialogHelper.MostrarAviso("Debe seleccionar un usuario para desactivar.");
                return;
            }

            /* Pide confirmación antes de desactivar */
            DialogResult resultado = DialogHelper.MostrarConfirmacion(
                $"¿Está seguro que desea desactivar al usuario {usuarioSeleccionado.NombreUsuario}?",
                "Confirmar desactivación");

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    /* Desactiva el usuario pasando también el ID del usuario actual */
                    usuarioManager.DesactivarUsuario(usuarioSeleccionado.Id,
                                                     AuthService.UsuarioActual.Id);

                    DialogHelper.MostrarExito("Usuario desactivado correctamente.");

                    /* Recarga la lista y limpia el panel */
                    CargarUsuarios();
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
         * Descripción: Reactiva un usuario desactivado.
         -----------------------------------------------------------------------------------------------------*/
        private void btnActivar_Click(object sender, EventArgs e)
        {
            /* Verifica que haya un usuario seleccionado */
            if (usuarioSeleccionado == null)
            {
                DialogHelper.MostrarAviso("Debe seleccionar un usuario para activar.");
                return;
            }

            /* Pide confirmación antes de activar */
            DialogResult resultado = DialogHelper.MostrarConfirmacion(
                            $"¿Está seguro que desea activar al usuario {usuarioSeleccionado.NombreUsuario}?",
                            "Confirmar activación");

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    /* Activa el usuario */
                    usuarioManager.ActivarUsuario(usuarioSeleccionado.Id);

                    DialogHelper.MostrarExito("Usuario activado correctamente.");

                    /* Recarga la lista y limpia el panel */
                    CargarUsuarios();
                    LimpiarPanel();
                }
                catch (Exception ex)
                {
                    DialogHelper.MostrarError(ex.Message);
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

                    DialogHelper.MostrarExito("Usuario modificado correctamente.");
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
                DialogHelper.MostrarError(ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
        * Evento: btnResetearClave_Click
        * Descripción: Resetea la contraseña del usuario seleccionado y genera una nueva temporal.
        * El usuario admin inicial (Id = 1) no puede resetearse.
        -----------------------------------------------------------------------------------------------------*/
        private void btnResetearClave_Click(object sender, EventArgs e)
        {
            /* Verifica que haya un usuario seleccionado */
            if (usuarioSeleccionado == null)
            {
                DialogHelper.MostrarAviso("Debe seleccionar un usuario para resetear la clave.");
                return;
            }

            /* El admin inicial no puede resetearse */
            if (usuarioSeleccionado.Id == 1)
            {
                DialogHelper.MostrarAviso("No se puede resetear la clave del administrador inicial del sistema.");
                return;
            }

            /* Pide confirmación antes de resetear */
            DialogResult resultado = DialogHelper.MostrarConfirmacion(
                $"¿Está seguro que desea resetear la clave de {usuarioSeleccionado.NombreUsuario}?\n" +
                $"El usuario deberá cambiarla en su próximo ingreso.",
                "Confirmar reseteo de clave");

            if (resultado == DialogResult.Yes)
            {
                try
                {
                    /* Resetea la clave y obtiene la nueva clave temporal */
                    string claveTemporal = authService.ResetearClave(usuarioSeleccionado.Id);

                    /* Muestra la nueva clave al admin */
                    MessageBox.Show(
                        $"Contraseña reseteada correctamente.\n\n" +
                        $"Usuario: {usuarioSeleccionado.NombreUsuario}\n" +
                        $"Contraseña temporal: {claveTemporal}\n\n" +
                        $"Comunique esta Contraseña al usuario.\n" +
                        $"Deberá cambiarla en su próximo ingreso.",
                        "Contraseña reseteada",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    /* Recarga la lista */
                    CargarUsuarios();
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
                DialogHelper.MostrarError("Error al cargar los roles: " + ex.Message);
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
                    int index = dgvUsuarios.Rows.Add(
                        u.Id,
                        u.NombreUsuario,
                        u.Nombre,
                        u.Apellido,
                        u.Rol?.Nombre,
                        u.Activo ? "Activo" : "Inactivo"
                    );

                    /* Colorea la celda de Estado según el valor */
                    if (u.Activo)
                    {
                        dgvUsuarios.Rows[index].Cells["Estado"].Style.BackColor = Color.LightGreen;
                        dgvUsuarios.Rows[index].Cells["Estado"].Style.ForeColor = Color.DarkGreen;
                    }
                    else
                    {
                        dgvUsuarios.Rows[index].Cells["Estado"].Style.BackColor = Color.LightCoral;
                        dgvUsuarios.Rows[index].Cells["Estado"].Style.ForeColor = Color.DarkRed;
                    }
                }
            }
            catch (Exception ex)
            {
                DialogHelper.MostrarError("Error al cargar los usuarios: " + ex.Message);
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
