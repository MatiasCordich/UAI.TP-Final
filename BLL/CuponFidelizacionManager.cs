using DAL.DAO;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    /* -----------------------------------------------------------------------------------------------------
     * Clase: CuponFidelizacionManager
     * Descripción: Capa de lógica de negocio para la gestión de cupones de fidelización.
     *              Gestiona el envío de cupones por cumpleaños o historial de compras.
     -----------------------------------------------------------------------------------------------------*/
    public class CuponFidelizacionManager
    {
        private CuponFidelizacionDAO cuponDAO = new CuponFidelizacionDAO();
        private ClienteDAO clienteDAO = new ClienteDAO();

        /* Días de vigencia de los cupones */
        private const int DiasVigenciaCupon = 30;

        /* Porcentaje de descuento de los cupones */
        private const decimal PorcentajeDescuentoCupon = 10m;

        /* -----------------------------------------------------------------------------------------------------
         * Función: EnviarCupon
         * Descripción: Genera y envía un cupón de descuento a un cliente (CU11).
         *              Valida que el cliente esté activo y tenga email para poder enviarlo.
         * Parámetros: ID del cliente y descripción del cupón.
         -----------------------------------------------------------------------------------------------------*/
        public void EnviarCupon(int idCliente, string descripcion)
        {
            try
            {
                /* Se verifica que el cliente exista y esté activo */
                Cliente cliente = clienteDAO.GetById(idCliente);
                if (cliente == null)
                    throw new Exception("El cliente no existe.");

                if (!cliente.Activo)
                    throw new Exception("El cliente no está activo.");

                /* Se verifica que el cliente tenga email para recibir el cupón */
                if (string.IsNullOrEmpty(cliente.Email))
                    throw new Exception("El cliente no tiene email registrado para recibir el cupón.");

                /* Se ejecutan las validaciones del para registrar el cupón. */
                ValidarCupon(idCliente, descripcion);

                /* Se verifica que el cliente no tenga ya un cupón vigente no usado */
                List<CuponFidelizacion> cuponesVigentes = cuponDAO.GetByCustomer(idCliente);
                if (cuponesVigentes.Count > 0)
                    throw new Exception("El cliente ya tiene un cupón vigente sin usar.");

                /* Se crea el cupón con vigencia de 30 días */
                CuponFidelizacion cupon = new CuponFidelizacion
                {
                    IdCliente = idCliente,
                    Descripcion = descripcion,
                    Descuento = PorcentajeDescuentoCupon,
                    FechaEmision = DateTime.Now,
                    FechaVencimiento = DateTime.Now.AddDays(DiasVigenciaCupon),
                    Enviado = true,
                    Usado = false
                };

                /* Guarda el cupón en el XML */
                cuponDAO.Insert(cupon);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al enviar el cupón: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: EnviarCuponesCumpleanos
         * Descripción: Genera y envía cupones automáticamente a los clientes que cumplen años hoy (CU11).
         -----------------------------------------------------------------------------------------------------*/
        public void EnviarCuponesCumpleanos()
        {
            try
            {
                /* Se obtienen los clientes que cumplen años hoy y tienen email */
                List<Cliente> clientesCumpleanos = clienteDAO.GetBirthdayToday();

                /* Envía un cupón a cada cliente que cumple años */
                foreach (Cliente cliente in clientesCumpleanos)
                    EnviarCupon(cliente.Id, $"¡Feliz cumpleaños {cliente.Nombre}! Tenes un {PorcentajeDescuentoCupon}% de descuento por tu día.");
            }
            catch (Exception ex)
            {
                throw new Exception("Error al enviar cupones de cumpleaños: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: ListarCupones
         * Descripción: Obtiene los cupones vigentes no usados de un cliente.
         * Parámetros: ID del cliente.
         * Retorna: lista de cupones vigentes.
         -----------------------------------------------------------------------------------------------------*/
        public List<CuponFidelizacion> ListarCupones(int idCliente)
        {
            try
            {
                /* Se verifica que el cliente exista */
                Cliente cliente = clienteDAO.GetById(idCliente);
                if (cliente == null)
                    throw new Exception("El cliente no existe.");

                /* Se filtran los cupones vigentes y no usados del cliente */
                return cuponDAO.GetByCustomer(idCliente);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los cupones: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: MarcarComoUsado
         * Descripción: Marca un cupón como usado al aplicarlo en una venta.
         * Parámetros: ID del cupón.
         -----------------------------------------------------------------------------------------------------*/
        public void MarcarComoUsado(int idCupon)
        {
            try
            {
                /* Se verifica que el cupón exista */
                CuponFidelizacion cupon = cuponDAO.GetById(idCupon);
                if (cupon == null)
                    throw new Exception("El cupón no existe.");

                /* Se verifica que no haya sido usado antes */
                if (cupon.Usado)
                    throw new Exception("El cupón ya fue utilizado.");

                /* Se verifica que el cupón no haya vencido */
                if (cupon.FechaVencimiento < DateTime.Today)
                    throw new Exception("El cupón ha vencido.");

                /* Se verifica que el cliente del cupón esté activo */
                Cliente cliente = clienteDAO.GetById(cupon.IdCliente);
                if (cliente == null || !cliente.Activo)
                    throw new Exception("El cliente del cupón no está activo.");

                /* Se marca el cupón como usado y lo actualiza en el XML */
                cupon.Usado = true;
                cuponDAO.Update(cupon);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al marcar el cupón como usado: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * FUNCIÓN PRIVADA
         -----------------------------------------------------------------------------------------------------*/
        /* -----------------------------------------------------------------------------------------------------
         * Función: ValidarCupon
         * Descripción: Centraliza las validaciones de negocio para el envío de cupones.
         *              Se llama en EnviarCupon antes de insertar.
         * Parámetros: ID del cliente y descripción del cupón.
         -----------------------------------------------------------------------------------------------------*/
        private void ValidarCupon(int idCliente, string descripcion)
        {
            /* Se valida que tenga cliente asignado */
            if (idCliente <= 0)
                throw new Exception("Debe asignar un cliente al cupón.");

            /* Se valida que la descripción no esté vacía */
            if (string.IsNullOrEmpty(descripcion))
                throw new Exception("La descripción del cupón es obligatoria.");

            /* Se valida que el porcentaje de descuento sea válido */
            if (PorcentajeDescuentoCupon <= 0 || PorcentajeDescuentoCupon > 100)
                throw new Exception("El porcentaje de descuento debe estar entre 1 y 100.");

            /* Se valida que la fecha de vencimiento sea posterior a la emisión */
            DateTime fechaVencimiento = DateTime.Now.AddDays(DiasVigenciaCupon);
            if (fechaVencimiento <= DateTime.Now)
                throw new Exception("La fecha de vencimiento debe ser posterior a la fecha de emisión.");
        }
    }
}
