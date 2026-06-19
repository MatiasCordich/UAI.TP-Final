using DAL.DAO;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL
{
    /* -----------------------------------------------------------------------------------------------------
     * Clase: ClienteManager
     * Descripción: Capa de lógica de negocio para la entidad Cliente.
     *              Gestiona el alta, baja, modificación y consulta de clientes.
     *              Los datos sensibles (Nombre, DNI, Email) se encriptan antes de guardar
     *              y se desencriptan al mostrar en pantalla (Desafío II).
     -----------------------------------------------------------------------------------------------------*/
    public class ClienteManager
    {
        private ClienteDAO clienteDAO = new ClienteDAO();

        /* -----------------------------------------------------------------------------------------------------
         * Función: ListarClientes
         * Descripción: Obtiene todos los clientes activos con sus datos desencriptados.
         * Retorna: lista de clientes.
         -----------------------------------------------------------------------------------------------------*/
        public List<Cliente> ListarClientes()
        {
            try
            {
                /* Obtiene todos los clientes activos del XML */
                List<Cliente> clientes = clienteDAO.GetAll();

                /* Desencripta los datos sensibles de cada cliente antes de mostrarlos */
                foreach (Cliente c in clientes)
                    DesencriptarDatosCliente(c);

                /* Devuelve los cientes. */
                return clientes;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los clientes: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: ObtenerCliente
         * Descripción: Obtiene un cliente por su ID con sus datos desencriptados.
         * Parámetros: ID del cliente.
         * Retorna: cliente encontrado.
         -----------------------------------------------------------------------------------------------------*/
        public Cliente ObtenerCliente(int id)
        {
            try
            {
                /* Busca el cliente por ID */
                Cliente cliente = clienteDAO.GetById(id);

                /* Si el cliente no existe, mostrar mensaje de error. */
                if (cliente == null)
                    throw new Exception("El cliente no existe.");

                /* Desencripta los datos sensibles antes de devolverlo */
                DesencriptarDatosCliente(cliente);

                /* Trae los datos de cliente. */
                return cliente;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el cliente: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: RegistrarCliente
         * Descripción: Da de alta un nuevo cliente validando y encriptando sus datos sensibles.
         * Parámetros: cliente con datos en texto plano.
         -----------------------------------------------------------------------------------------------------*/
        public void RegistrarCliente(Cliente cliente)
        {
            try
            {
                /* Ejecuta las validaciones de negocio */
                ValidarCliente(cliente);

                /* Verifica que no exista un cliente con el mismo DNI (encriptado para comparar) */
                Cliente existente = clienteDAO.GetByDni(EncryptService.Encriptar(cliente.Dni));
                if (existente != null)
                    throw new Exception("Ya existe un cliente con ese DNI.");

                /* Encripta los datos sensibles antes de guardar en el XML */
                EncriptarDatosCliente(cliente);
                cliente.Activo = true;
                cliente.FechaAlta = DateTime.Now;
                clienteDAO.Insert(cliente);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el cliente: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: ModificarCliente
         * Descripción: Modifica los datos de un cliente validando y encriptando sus datos sensibles.
         * Parámetros: cliente con datos actualizados en texto plano.
         -----------------------------------------------------------------------------------------------------*/
        public void ModificarCliente(Cliente cliente)
        {
            try
            {
                /* Verifica que el cliente exista y esté activo */
                Cliente existente = clienteDAO.GetById(cliente.Id);
                if (existente == null)
                    throw new Exception("El cliente no existe.");
                if (!existente.Activo)
                    throw new Exception("No se puede modificar un cliente desactivado.");

                /* Ejecuta las validaciones de negocio */
                ValidarCliente(cliente);

                /* Encripta los datos sensibles antes de actualizar en el XML */
                EncriptarDatosCliente(cliente);
                clienteDAO.Update(cliente);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar el cliente: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: DesactivarCliente
         * Descripción: Baja lógica del cliente. Verifica que exista antes de desactivarlo.
         * Parámetros: ID del cliente.
         -----------------------------------------------------------------------------------------------------*/
        public void DesactivarCliente(int id)
        {
            try
            {
                /* Verifica que el cliente exista */
                Cliente cliente = clienteDAO.GetById(id);
                if (cliente == null)
                    throw new Exception("El cliente no existe.");

                /* Se valida que el clienta no esté desactivado previamente. */
                if (!cliente.Activo)
                    throw new Exception("El cliente ya está desactivado.");

                /* Baja lógica: el cliente queda en el XML pero con Activo = false */
                clienteDAO.Deactivate(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al desactivar el cliente: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: ActivarCliente
         * Descripción: Reactiva un cliente previamente desactivado.
         * Parámetros: ID del cliente.
         -----------------------------------------------------------------------------------------------------*/
        public void ActivarCliente(int id)
        {
            try
            {
                /* Verifica que el cliente exista */
                Cliente cliente = clienteDAO.GetById(id);
                if (cliente == null)
                    throw new Exception("El cliente no existe.");

                /* Se valida que el clienta no esté activo previamente. */
                if (cliente.Activo)
                    throw new Exception("El cliente ya está activo.");

                /* Reactiva el cliente en el XML */
                cliente.Activo = true;
                clienteDAO.Update(cliente);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al activar el cliente: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
        MÉTODOS PRIVADOS
         -----------------------------------------------------------------------------------------------------*/

        /* -----------------------------------------------------------------------------------------------------
         * Función: ValidarCliente
         * Descripción: Centraliza todas las validaciones de negocio para alta y modificación.
         *              Se llama tanto en RegistrarCliente como en ModificarCliente.
         * Parámetros: Cliente con datos en texto plano.
         -----------------------------------------------------------------------------------------------------*/
        private void ValidarCliente(Cliente cliente)
        {
            /* Nombre obligatorio y solo letras/espacios */
            if (string.IsNullOrEmpty(cliente.Nombre))
                throw new Exception("El nombre es obligatorio.");
            if (!Regex.IsMatch(cliente.Nombre, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
                throw new Exception("El nombre solo puede contener letras y espacios.");

            /* Apellido obligatorio y solo letras/espacios */
            if (string.IsNullOrEmpty(cliente.Apellido))
                throw new Exception("El apellido es obligatorio.");
            if (!Regex.IsMatch(cliente.Apellido, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
                throw new Exception("El apellido solo puede contener letras y espacios.");

            /* DNI obligatorio, solo números, entre 7 y 8 dígitos */
            if (string.IsNullOrEmpty(cliente.Dni))
                throw new Exception("El DNI es obligatorio.");
            if (!Regex.IsMatch(cliente.Dni, @"^\d{7,8}$"))
                throw new Exception("El DNI debe contener entre 7 y 8 dígitos numéricos.");

            /* Email opcional pero con formato válido si se ingresa */
            if (!string.IsNullOrEmpty(cliente.Email) &&
                !Regex.IsMatch(cliente.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new Exception("El formato del email no es válido.");

            /* FechaNacimiento opcional pero con validaciones si se ingresa */
            if (cliente.FechaNacimiento.HasValue)
            {
                /* No puede ser una fecha futura */
                if (cliente.FechaNacimiento.Value > DateTime.Today)
                    throw new Exception("La fecha de nacimiento no puede ser una fecha futura.");

                /* No puede indicar una edad mayor a 120 años */
                int edad = DateTime.Today.Year - cliente.FechaNacimiento.Value.Year;
                if (edad > 120)
                    throw new Exception("La fecha de nacimiento no es válida.");
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: EncriptarDatosCliente
         * Descripción: Encripta los datos sensibles del cliente. 
         * Parámetros: Cliente con datos en texto plano.
         -----------------------------------------------------------------------------------------------------*/
        private void EncriptarDatosCliente(Cliente cliente)
        {
            cliente.Nombre = EncryptService.Encriptar(cliente.Nombre);
            cliente.Dni = EncryptService.Encriptar(cliente.Dni);
            cliente.Email = EncryptService.Encriptar(cliente.Email);
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: DesencriptarDatosCliente
         * Descripción: Desencripta los datos sensibles del cliente
         * Parámetros: Cliente con datos encriptados.
         -----------------------------------------------------------------------------------------------------*/
        private void DesencriptarDatosCliente(Cliente cliente)
        {
            cliente.Nombre = EncryptService.Desencriptar(cliente.Nombre);
            cliente.Dni = EncryptService.Desencriptar(cliente.Dni);
            cliente.Email = EncryptService.Desencriptar(cliente.Email);
        }
    }
}
