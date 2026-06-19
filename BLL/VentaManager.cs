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
     * Clase: VentaManager
     * Descripción: Capa de lógica de negocio para la entidad Venta.
     *              Gestiona el registro de ventas, aplicación de beneficios y cambios de calzado.
     -----------------------------------------------------------------------------------------------------*/
    public class VentaManager
    {
        private VentaDAO ventaDAO = new VentaDAO();
        private VentaProductoDAO ventaProductoDAO = new VentaProductoDAO();
        private CambioCalzadoDAO cambioCalzadoDAO = new CambioCalzadoDAO();
        private ProductoManager productoManager = new ProductoManager();

        /* Plazo en días para aceptar cambios de calzado (CU03) */
        private const int PlazosCambiosDias = 30;

        /* Porcentaje de descuento para jubilados (CU02) */
        private const decimal PorcentajeDescuentoJubilado = 15m;

        /* Medios de pago válidos */
        private static readonly List<string> MediosPagoValidos = new List<string>
        {
            "Efectivo", "Debito", "Credito", "Transferencia"
        };

        /* -----------------------------------------------------------------------------------------------------
         * Función: RegistrarVenta
         * Descripción: Registra una nueva venta, descuenta el stock y genera la factura (CU01).
         * Parámetros: venta con su detalle de productos.
         -----------------------------------------------------------------------------------------------------*/
        public void RegistrarVenta(Venta venta, List<VentaProducto> detalle)
        {
            try
            {
                /* Valida que tenga un cliente asignado */
                if (venta.IdCliente <= 0)
                    throw new Exception("Debe asignar un cliente a la venta.");

                /* Valida que el medio de pago no esté vacío */
                if (string.IsNullOrEmpty(venta.MedioPago))
                    throw new Exception("El medio de pago es obligatorio.");

                /* Valida que el medio de pago sea uno de los valores válidos */
                if (!MediosPagoValidos.Contains(venta.MedioPago))
                    throw new Exception("El medio de pago debe ser: Efectivo, Debito, Credito o Transferencia.");

                /* Valida que la venta tenga al menos un producto */
                if (detalle == null || detalle.Count == 0)
                    throw new Exception("La venta debe tener al menos un producto.");

                /* Calcula el total sumando precio x cantidad de cada producto */
                venta.Total = CalcularTotal(detalle);

                /* Valida que el total sea mayor a cero */
                if (venta.Total <= 0)
                    throw new Exception("El total de la venta debe ser mayor a cero.");

                /* La fecha se asigna automáticamente */
                venta.Fecha = DateTime.Now;
                venta.BeneficioAplicado = false;
                venta.Descuento = 0;

                /* Guarda la venta en el XML */
                ventaDAO.Insert(venta);

                /* Guarda el detalle y descuenta el stock de cada producto */
                foreach (VentaProducto vp in detalle)
                {
                    /* Asocia el detalle a la venta recién creada */
                    vp.IdVenta = venta.Id;
                    ventaProductoDAO.Insert(vp);

                    /* Descuenta el stock del producto vendido */
                    Producto producto = productoManager.ObtenerProducto(vp.IdProducto);
                    productoManager.AjustarStock(producto, -vp.Cantidad);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar la venta: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: AplicarBeneficioJubilado
         * Descripción: Aplica el descuento jubilatorio a una venta. No es acumulable (CU02).
         * Parámetros: venta a la que se aplica el beneficio y cliente asociado.
         -----------------------------------------------------------------------------------------------------*/
        public void AplicarBeneficioJubilado(Venta venta, Cliente cliente)
        {
            try
            {
                /* Verifica que el cliente sea jubilado */
                if (!cliente.EsJubilado)
                    throw new Exception("El cliente no tiene habilitado el beneficio jubilatorio.");

                /* Verifica que el beneficio no haya sido aplicado antes (no acumulable) */
                if (venta.BeneficioAplicado)
                    throw new Exception("El beneficio jubilatorio ya fue aplicado a esta venta.");

                /* Calcula el monto del descuento */
                decimal montoDescuento = venta.Total * (PorcentajeDescuentoJubilado / 100);

                /* Verifica que el descuento no deje el total en cero o negativo */
                if (venta.Total - montoDescuento <= 0)
                    throw new Exception("El descuento no puede dejar el total en cero o negativo.");

                /* Aplica el descuento sobre el total */
                venta.Total -= montoDescuento;
                venta.Descuento = PorcentajeDescuentoJubilado;
                venta.BeneficioAplicado = true;

                /* Guarda la venta actualizada en el XML */
                ventaDAO.Update(venta);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al aplicar el beneficio jubilatorio: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: RegistrarCambio
         * Descripción: Registra un cambio de calzado en postventa validando el plazo (CU03).
         *              Devuelve el producto viejo al stock y descuenta el producto nuevo.
         * Parámetros: ID de la venta original, producto viejo y producto nuevo.
         -----------------------------------------------------------------------------------------------------*/
        public void RegistrarCambio(int idVenta, int idProductoViejo, int idProductoNuevo)
        {
            try
            {
                /* Valida que la venta original exista */
                Venta venta = ventaDAO.GetById(idVenta);
                if (venta == null)
                    throw new Exception("El ticket de venta no existe.");

                /* Verifica que esté dentro del plazo de cambio permitido */
                int diasTranscurridos = (DateTime.Today - venta.Fecha.Date).Days;
                if (diasTranscurridos > PlazosCambiosDias)
                    throw new Exception($"El plazo de cambio de {PlazosCambiosDias} días ha vencido.");

                /* Verifica que el producto nuevo sea diferente al viejo */
                if (idProductoViejo == idProductoNuevo)
                    throw new Exception("El producto nuevo debe ser diferente al producto a cambiar.");

                /* Devuelve el producto viejo al stock (+1) */
                Producto productoViejo = productoManager.ObtenerProducto(idProductoViejo);
                productoManager.AjustarStock(productoViejo, +1);

                /* Descuenta el producto nuevo del stock (-1) */
                Producto productoNuevo = productoManager.ObtenerProducto(idProductoNuevo);
                productoManager.AjustarStock(productoNuevo, -1);

                /* Registra el cambio con trazabilidad en el XML */
                CambioCalzado cambio = new CambioCalzado
                {
                    IdVentaOriginal = idVenta,
                    IdProductoViejo = idProductoViejo,
                    IdProductoNuevo = idProductoNuevo,
                    Fecha = DateTime.Now
                };
                cambioCalzadoDAO.Insert(cambio);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el cambio: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: ObtenerVentasPorCliente
         * Descripción: Obtiene todas las ventas de un cliente específico.
         * Parámetros: ID del cliente.
         * Retorna: lista de ventas del cliente.
         -----------------------------------------------------------------------------------------------------*/
        public List<Venta> ObtenerVentasPorCliente(int idCliente)
        {
            try
            {
                /* Filtra las ventas por cliente en el XML */
                return ventaDAO.GetByCustomer(idCliente);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las ventas del cliente: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: CalcularTotal
         * Descripción: Calcula el total de una venta sumando precio x cantidad de cada producto.
         * Parámetros: detalle de productos de la venta.
         * Retorna: total calculado.
         -----------------------------------------------------------------------------------------------------*/
        public decimal CalcularTotal(List<VentaProducto> detalle)
        {
            try
            {
                decimal total = 0;

                /* Suma precio x cantidad por cada línea del detalle */
                foreach (VentaProducto vp in detalle)
                {
                    Producto producto = productoManager.ObtenerProducto(vp.IdProducto);
                    total += producto.Precio * vp.Cantidad;
                }
                return total;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al calcular el total: " + ex.Message);
            }
        }
    }
}
