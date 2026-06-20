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
     * Clase: PagoManager
     * Descripción: Capa de lógica de negocio para la gestión de pagos a proveedores.
     *              Gestiona el registro de pagos, generación de recibos y conciliación diaria.
     -----------------------------------------------------------------------------------------------------*/
    public class PagoManager
    {
        private PagoDAO pagoDAO = new PagoDAO();
        private ReciboDAO reciboDAO = new ReciboDAO();
        private ProveedorDAO proveedorDAO = new ProveedorDAO();
        private PedidoCompraDAO pedidoDAO = new PedidoCompraDAO();
        private ConciliacionDiariaDAO conciliacionDAO = new ConciliacionDiariaDAO();

        /* Medios de pago válidos para pagos a proveedores */
        private static readonly List<string> MediosPagoValidos = new List<string>
        {
            "Efectivo", "Transferencia", "Cheque"
        };

        /* -----------------------------------------------------------------------------------------------------
         * Función: RegistrarPago
         * Descripción: Registra un pago a un proveedor validando las reglas de negocio (CU06).
         *              Genera el recibo automáticamente y actualiza la deuda del proveedor.
         * Parámetros: pago con monto, medio de pago e IDs de proveedor y pedido.
         -----------------------------------------------------------------------------------------------------*/
        public void RegistrarPago(Pago pago)
        {
            try
            {
                /* Se realizan las validaciones con respecto al Pago. */
                ValidarPago(pago);

                /* Se verifica que el proveedor exista */
                Proveedor proveedor = proveedorDAO.GetById(pago.IdProveedor);
                if (proveedor == null)
                    throw new Exception("El proveedor no existe.");

                /* Se verifica que el pedido exista y esté en estado Recibido */
                PedidoCompra pedido = pedidoDAO.GetById(pago.IdPedido);
                if (pedido == null)
                    throw new Exception("El pedido no existe.");

                if (pedido.Estado != "Recibido")
                    throw new Exception("Solo se pueden pagar pedidos en estado Recibido.");

                /* Se valida que el monto no supere la deuda del proveedor */
                if (pago.Monto > proveedor.DeudaTotal)
                    throw new Exception("El monto del pago no puede superar la deuda del proveedor.");

                /* La fecha se asigna automáticamente */
                pago.Fecha = DateTime.Now;

                /* Registra el pago en el XML */
                pagoDAO.Insert(pago);

                /* Genera el recibo automáticamente al confirmar el pago. */
                GenerarRecibo(pago.Id);

                /* Descuenta el monto pagado de la deuda del proveedor. */
                proveedor.DeudaTotal -= pago.Monto;

                /* Se modifica el dato de la deuda del proveedor en el XML. */
                proveedorDAO.Update(proveedor);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el pago: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: GenerarRecibo
         * Descripción: Genera automáticamente un recibo digital al confirmar un pago (CU07).
         *              Verifica que no exista ya un recibo para ese pago.
         * Parámetros: ID del pago confirmado.
         -----------------------------------------------------------------------------------------------------*/
        public void GenerarRecibo(int idPago)
        {
            try
            {
                /* Se valida que el ID del pago sea válido */
                if (idPago <= 0)
                    throw new Exception("El ID del pago no es válido.");

                /* Se verifica que no exista ya un recibo para ese pago */
                Recibo reciboExistente = reciboDAO.GetByPayment(idPago);
                if (reciboExistente != null)
                    throw new Exception("Ya existe un recibo para ese pago.");

                /* Se crea el recibo con número único y fecha de emisión automática */
                Recibo recibo = new Recibo
                {
                    IdPago = idPago,
                    FechaEmision = DateTime.Now,
                    NumeroRecibo = GenerarNumeroRecibo()
                };

                /* Guarda el recibo en el XML */
                reciboDAO.Insert(recibo);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar el recibo: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: ConciliarPagos
         * Descripción: Compara los pagos registrados con los egresos reales de caja/banco (CU08).
         *              Si hay diferencias genera una alerta. Guarda el informe de conciliación.
         * Parámetros: fecha de conciliación y total de egresos reales de caja/banco.
         -----------------------------------------------------------------------------------------------------*/
        public ConciliacionDiaria ConciliarPagos(DateTime fecha, decimal totalEgresos)
        {
            try
            {
                /* Se valida que la fecha no sea futura */
                if (fecha.Date > DateTime.Today)
                    throw new Exception("La fecha de conciliación no puede ser futura.");

                /* Se valida que el total de egresos no sea negativo */
                if (totalEgresos < 0)
                    throw new Exception("El total de egresos no puede ser negativo.");

                /* Se verifica que no exista ya una conciliación para esa fecha. */
                ConciliacionDiaria existente = conciliacionDAO.GetByDate(fecha);
                if (existente != null)
                    throw new Exception("Ya existe una conciliación registrada para esa fecha.");

                /* Obtiene todos los pagos registrados para la fecha indicada */
                List<Pago> pagosDia = pagoDAO.GetByDate(fecha);

                /* Suma el total de pagos del día */
                decimal totalPagos = 0;
                foreach (Pago p in pagosDia)
                {
                    totalPagos += p.Monto;
                }
                    
                /* Calcula la diferencia entre lo registrado y los egresos reales */
                decimal diferencia = totalPagos - totalEgresos;
                bool tieneAlerta = diferencia != 0;

                /* Crea y guarda el informe de conciliación */
                ConciliacionDiaria conciliacion = new ConciliacionDiaria
                {
                    Fecha = fecha,
                    TotalPagos = totalPagos,
                    TotalEgresos = totalEgresos,
                    TieneAlerta = tieneAlerta,
                    /* Si hay diferencia agrega una observación con el monto */
                    Observacion = tieneAlerta ? $"Diferencia detectada: {diferencia:C}" : null
                };

                /* Registrar la conciliación en el XML. */
                conciliacionDAO.Insert(conciliacion);

                return conciliacion;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al conciliar los pagos: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: ValidarPago
         * Descripción: Centraliza todas las validaciones de negocio para el registro de pagos.
         * Parámetros: pago con datos en texto plano.
         -----------------------------------------------------------------------------------------------------*/
        private void ValidarPago(Pago pago)
        {
            /* Valida que tenga proveedor asignado */
            if (pago.IdProveedor <= 0)
                throw new Exception("Debe asignar un proveedor al pago.");

            /* Valida que tenga pedido asignado */
            if (pago.IdPedido <= 0)
                throw new Exception("Debe asignar un pedido al pago.");

            /* Valida que el monto sea mayor a cero */
            if (pago.Monto <= 0)
                throw new Exception("El monto del pago debe ser mayor a cero.");

            /* Valida que el medio de pago no esté vacío */
            if (string.IsNullOrEmpty(pago.MedioPago))
                throw new Exception("El medio de pago es obligatorio.");

            /* Valida que el medio de pago sea uno de los valores válidos */
            if (!MediosPagoValidos.Contains(pago.MedioPago))
                throw new Exception("El medio de pago debe ser: Efectivo, Transferencia o Cheque.");
        }

         /* -----------------------------------------------------------------------------------------------------
         * FUNCIÓN PRIVADA
         -----------------------------------------------------------------------------------------------------*/
        /* -----------------------------------------------------------------------------------------------------
         * Función: GenerarNumeroRecibo
         * Descripción: Genera un número de recibo único basado en fecha y hora.
         * Retorna: número de recibo único con formato REC-yyyyMMddHHmmss.
         -----------------------------------------------------------------------------------------------------*/
        private string GenerarNumeroRecibo()
        {
            /* Usa la fecha y hora actual para garantizar unicidad */
            return "REC-" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }
    }
}
