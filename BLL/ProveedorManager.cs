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
     * Clase: ProveedorManager
     * Descripción: Capa de lógica de negocio para la entidad Proveedor.
     *              Gestiona el alta, baja, modificación y consulta de proveedores.
     *              El CUIT se encripta antes de guardar (Desafío II).
     -----------------------------------------------------------------------------------------------------*/
    public class ProveedorManager
    {
        private ProveedorDAO proveedorDAO = new ProveedorDAO();
        private PedidoCompraDAO pedidoDAO = new PedidoCompraDAO();

        /* -----------------------------------------------------------------------------------------------------
         * Función: ListarProveedores
         * Descripción: Obtiene todos los proveedores activos con sus datos desencriptados.
         * Retorna: lista de proveedores.
         -----------------------------------------------------------------------------------------------------*/
        public List<Proveedor> ListarProveedoresOFiltrados(string nombre = null, bool? activo = null)
        {
            try
            {
                /* Obtiene los proveedores del XML aplicando los filtros opcionales */
                List<Proveedor> proveedores = proveedorDAO.GetAllOrFiltered(nombre, activo);

                /* Desencripta el CUIT de cada proveedor antes de mostrarlo */
                foreach (Proveedor p in proveedores)
                    p.Cuit = EncryptService.Desencriptar(p.Cuit);
                return proveedores;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los proveedores: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: ObtenerProveedor
         * Descripción: Obtiene un proveedor por su ID con sus datos desencriptados.
         * Parámetros: ID del proveedor.
         * Retorna: proveedor encontrado.
         -----------------------------------------------------------------------------------------------------*/
        public Proveedor ObtenerProveedor(int id)
        {
            try
            {
                /* Busca el proveedor por ID */
                Proveedor proveedor = proveedorDAO.GetById(id);
                if (proveedor == null)
                    throw new Exception("El proveedor no existe.");

                /* Desencripta el CUIT antes de devolverlo */
                proveedor.Cuit = EncryptService.Desencriptar(proveedor.Cuit);
                return proveedor;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el proveedor: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: RegistrarProveedor
         * Descripción: Da de alta un nuevo proveedor validando y encriptando su CUIT (Desafío II).
         * Parámetros: proveedor con datos en texto plano.
         -----------------------------------------------------------------------------------------------------*/
        public void RegistrarProveedor(Proveedor proveedor)
        {
            try
            {
                /* Se realizan las validaciones correspondientes para insertar un proveedor. */
                ValidarProveedor(proveedor);

                /* Se verifica que no exista otro proveedor (activo o inactivo) con el mismo CUIT */
                if (ExisteCuit(proveedor.Cuit, 0))
                    throw new Exception("Ya existe un proveedor con ese CUIT.");

                /* Encripta el CUIT antes de guardar en el XML */
                proveedor.Cuit = EncryptService.Encriptar(proveedor.Cuit);
                proveedor.Activo = true;
                proveedor.DeudaTotal = 0;

                /* Se llama al DAO para insertar el proveedor al XML */
                proveedorDAO.Insert(proveedor);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el proveedor: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: ModificarProveedor
         * Descripción: Modifica los datos de un proveedor validando y encriptando su CUIT.
         * Parámetros: proveedor con datos actualizados en texto plano.
         -----------------------------------------------------------------------------------------------------*/
        public void ModificarProveedor(Proveedor proveedor)
        {
            try
            {
                /* Verifica que el proveedor exista y esté activo */
                Proveedor existente = proveedorDAO.GetById(proveedor.Id);
                if (existente == null)
                    throw new Exception("El proveedor no existe.");

                if (!existente.Activo)
                    throw new Exception("No se puede modificar un proveedor desactivado.");

                /* La deuda total es la cuenta corriente del proveedor: la modifican los pedidos y los pagos,
                   nunca el formulario de proveedores. Se conserva el valor que ya tiene guardado. */
                proveedor.DeudaTotal = existente.DeudaTotal;

                /* Se realizan las validaciones correspondiente a los datos del proveedor. */
                ValidarProveedor(proveedor);

                /* Se verifica que el CUIT no pertenezca a otro proveedor */
                if (ExisteCuit(proveedor.Cuit, proveedor.Id))
                    throw new Exception("Ya existe otro proveedor con ese CUIT.");

                /* Encripta el CUIT antes de actualizar en el XML */
                proveedor.Cuit = EncryptService.Encriptar(proveedor.Cuit);

                /* Se ejecuta el DAO para modifcar los datos del provedor en el XML */
                proveedorDAO.Update(proveedor);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar el proveedor: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: ObtenerPedidosPorProveedor
         * Descripción: Obtiene todos los pedidos de compra de un proveedor específico.
         * Parámetros: ID del proveedor.
         * Retorna: lista de pedidos del proveedor.
         -----------------------------------------------------------------------------------------------------*/
        public List<PedidoCompra> ObtenerPedidosPorProveedor(int idProveedor)
        {
            try
            {
                /* Filtra los pedidos por proveedor en el XML */
                return pedidoDAO.GetBySupplier(idProveedor);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los pedidos del proveedor: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: DesactivarProveedor
         * Descripción: Baja lógica del proveedor. Verifica que exista antes de desactivarlo.
         * Parámetros: ID del proveedor.
         -----------------------------------------------------------------------------------------------------*/
        public void DesactivarProveedor(int id)
        {
            try
            {
                /* Verifica que el proveedor exista */
                Proveedor proveedor = proveedorDAO.GetById(id);
                if (proveedor == null)
                    throw new Exception("El proveedor no existe.");

                /* Se verifica que el proveedor no esté desactivado previamente. */
                if (!proveedor.Activo)
                    throw new Exception("El proveedor ya está desactivado.");

                /* No se puede desactivar un proveedor con deuda pendiente: al no estar activo dejaría de
                   aparecer en el módulo de pagos y la deuda no se podría saldar. */
                if (proveedor.DeudaTotal > 0)
                    throw new Exception("No se puede desactivar un proveedor con deuda pendiente. Primero debe saldarse.");

                /* Baja lógica: el proveedor queda en el XML pero con Activo = false */
                proveedorDAO.Deactivate(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al desactivar el proveedor: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: ActivarProveedor
         * Descripción: Reactiva un proveedor previamente desactivado.
         * Parámetros: ID del proveedor.
         -----------------------------------------------------------------------------------------------------*/
        public void ActivarProveedor(int id)
        {
            try
            {
                /* Verifica que el proveedor exista */
                Proveedor proveedor = proveedorDAO.GetById(id);
                if (proveedor == null)
                    throw new Exception("El proveedor no existe.");

                /* Se verifica que el proveedor no esté activo previamente. */
                if (proveedor.Activo)
                    throw new Exception("El proveedor ya está activo.");

                /* Reactiva el proveedor en el XML */
                proveedor.Activo = true;
                proveedorDAO.Update(proveedor);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al activar el proveedor: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: ExisteCuit
         * Descripción: Verifica si ya existe un proveedor (activo o inactivo) con el mismo CUIT.
         *              Compara sin tener en cuenta los guiones, para que 20-12345678-9 y 20123456789
         *              se consideren el mismo CUIT.
         * Parámetros: cuit en texto plano e id del proveedor que se debe excluir de la búsqueda
         *             (el propio proveedor al modificar; 0 al registrar uno nuevo).
         * Retorna: true si el CUIT ya está en uso por otro proveedor.
         -----------------------------------------------------------------------------------------------------*/
        private bool ExisteCuit(string cuit, int idExcluido)
        {
            /* Se quitan los guiones del CUIT ingresado para poder compararlo. */
            string cuitBuscado = cuit.Replace("-", "");

            /* Se recorren todos los proveedores, activos e inactivos. */
            foreach (Proveedor p in proveedorDAO.GetAllOrFiltered())
            {
                /* El proveedor excluido (él mismo al modificar) no cuenta como duplicado. */
                if (p.Id == idExcluido)
                    continue;

                /* El CUIT está encriptado en el XML: se desencripta para compararlo. */
                if (EncryptService.Desencriptar(p.Cuit).Replace("-", "") == cuitBuscado)
                    return true;
            }

            return false;
        }



        /* -----------------------------------------------------------------------------------------------------
         * Función: ValidarProveedor
         * Descripción: Centraliza todas las validaciones de negocio para alta y modificación.
         *              Se llama tanto en RegistrarProveedor como en ModificarProveedor.
         * Parámetros: proveedor con datos en texto plano.
         -----------------------------------------------------------------------------------------------------*/
        private void ValidarProveedor(Proveedor proveedor)
        {
            /* Nombre obligatorio: letras, números, espacios y los símbolos . , & - */
            if (string.IsNullOrEmpty(proveedor.Nombre))
                throw new Exception("El nombre es obligatorio.");
            if (!Regex.IsMatch(proveedor.Nombre, @"^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\s.,&-]+$"))
                throw new Exception("El nombre solo puede contener letras, números, espacios y los símbolos . , & -");

            /* CUIT obligatorio con formato válido: XX-XXXXXXXX-X o 11 dígitos sin guiones */
            if (string.IsNullOrEmpty(proveedor.Cuit))
                throw new Exception("El CUIT es obligatorio.");
            if (!Regex.IsMatch(proveedor.Cuit, @"^\d{2}-\d{8}-\d{1}$") &&
                !Regex.IsMatch(proveedor.Cuit, @"^\d{11}$"))
                throw new Exception("El CUIT debe tener el formato XX-XXXXXXXX-X o 11 dígitos numéricos.");

            /* Email opcional pero con formato válido si se ingresa */
            if (!string.IsNullOrEmpty(proveedor.Email) &&
                !Regex.IsMatch(proveedor.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new Exception("El formato del email no es válido.");

            /* DeudaTotal no puede ser negativa */
            if (proveedor.DeudaTotal < 0)
                throw new Exception("La deuda total no puede ser negativa.");
        }
    }
}
