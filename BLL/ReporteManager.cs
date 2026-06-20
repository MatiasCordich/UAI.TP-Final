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
     * Clase: ReporteManager
     * Descripción: Capa de lógica de negocio para la generación de reportes y dashboard.
     *              Concentra toda la información clave para la toma de decisiones del dueño (CU12).
     -----------------------------------------------------------------------------------------------------*/
    public class ReporteManager
    {
        /* Instancia del DAO de ventas para consultar ventas */
        private VentaDAO ventaDAO = new VentaDAO();

        /* Instancia del DAO de detalle de venta para consultar productos vendidos */
        private VentaProductoDAO ventaProductoDAO = new VentaProductoDAO();

        /* Instancia del DAO de productos para obtener modelo y talle */
        private ProductoDAO productoDAO = new ProductoDAO();

        /* Instancia del DAO de cambios para consultar devoluciones */
        private CambioCalzadoDAO cambioDAO = new CambioCalzadoDAO();

        /* Instancia del DAO de pagos para consultar egresos */
        private PagoDAO pagoDAO = new PagoDAO();

        /* -----------------------------------------------------------------------------------------------------
         * Función: ObtenerVentasPorTalle
         * Descripción: Agrupa las ventas por talle en un período (dashboard CU12).
         * Parámetros: fecha desde y fecha hasta del período.
         * Retorna: diccionario con talle y total vendido. Vacío si no hay ventas.
         -----------------------------------------------------------------------------------------------------*/
        public Dictionary<int, int> ObtenerVentasPorTalle(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                /* Se valida que el período sea correcto antes de consultar */
                ValidarPeriodo(fechaDesde, fechaHasta);

                /* Se obtiene la lista de ventas que ocurrieron dentro del período indicado */
                List<Venta> ventas = ventaDAO.GetByPeriod(fechaDesde, fechaHasta);

                /* Se crea el diccionario donde la clave es el talle y el valor es la cantidad total vendida */
                Dictionary<int, int> ventasPorTalle = new Dictionary<int, int>();

                /* Si no hay ventas en el período devuelve el diccionario vacío sin lanzar error */
                if (ventas.Count == 0)
                    return ventasPorTalle;

                /* Recorre cada venta del período */
                foreach (Venta venta in ventas)
                {
                    /* Por cada venta, se obtiene el detalle de productos de esa venta */
                    List<VentaProducto> detalle = ventaProductoDAO.GetBySale(venta.Id);

                    /* Se recorre cada línea del detalle */
                    foreach (VentaProducto vp in detalle)
                    {
                        /* Se obtiene el producto para saber su talle */
                        Producto producto = productoDAO.GetById(vp.IdProducto);

                        /* Solo procesa si el producto existe */
                        if (producto != null)
                        {
                            /* Si el talle no existe en el diccionario lo agrega con valor 0 */
                            if (!ventasPorTalle.ContainsKey(producto.Talle))
                            {
                                ventasPorTalle[producto.Talle] = 0;
                            }

                            /* Se suma la cantidad vendida de ese talle */
                            ventasPorTalle[producto.Talle] += vp.Cantidad;
                        }
                    }
                }

                /* Devuelve el diccionario con los totales por talle */
                return ventasPorTalle;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener ventas por talle: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: ObtenerRotacionPorModelo
         * Descripción: Agrupa las ventas por modelo ordenadas por mayor cantidad vendida (CU12).
         * Parámetros: fecha desde y fecha hasta del período.
         * Retorna: diccionario con modelo y total vendido ordenado descendente. Vacío si no hay ventas.
         -----------------------------------------------------------------------------------------------------*/
        public Dictionary<string, int> ObtenerRotacionPorModelo(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                /* Se valida que el período sea correcto antes de consultar */
                ValidarPeriodo(fechaDesde, fechaHasta);

                /* Se obtiene la lista de ventas que ocurrieron dentro del período indicado */
                List<Venta> ventas = ventaDAO.GetByPeriod(fechaDesde, fechaHasta);

                /* Se crea el diccionario donde la clave es el modelo y el valor es la cantidad total vendida */
                Dictionary<string, int> rotacion = new Dictionary<string, int>();

                /* Si no hay ventas en el período devuelve el diccionario vacío sin lanzar error */
                if (ventas.Count == 0)
                    return rotacion;

                /* Se recorre cada venta del período */
                foreach (Venta venta in ventas)
                {
                    /* Se obtiene el detalle de productos de esa venta */
                    List<VentaProducto> detalle = ventaProductoDAO.GetBySale(venta.Id);

                    /* Recorre cada línea del detalle */
                    foreach (VentaProducto vp in detalle)
                    {
                        /* Se obtiene el producto para saber su modelo */
                        Producto producto = productoDAO.GetById(vp.IdProducto);

                        /* Solo procesa si el producto existe */
                        if (producto != null)
                        {
                            /* Si el modelo no existe en el diccionario lo agrega con valor 0 */
                            if (!rotacion.ContainsKey(producto.Modelo))
                            {
                                rotacion[producto.Modelo] = 0;
                            }
                                
                            /* Se suma la cantidad vendida de ese modelo */
                            rotacion[producto.Modelo] += vp.Cantidad;
                        }
                    }
                }

                /* Se ordena el diccionario de mayor a menor cantidad vendida y lo devuelve */
                return rotacion.OrderByDescending(r => r.Value)
                               .ToDictionary(r => r.Key, r => r.Value);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener rotación por modelo: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: ObtenerEstadoStock
         * Descripción: Obtiene el estado actual del stock ordenado por modelo y talle (CU12).
         * Retorna: lista de productos con su estado de stock. Vacía si no hay productos.
         -----------------------------------------------------------------------------------------------------*/
        public List<Producto> ObtenerEstadoStock()
        {
            try
            {
                /* Se obtienen todos los productos activos del XML */
                /* Los ordena primero por modelo y luego por talle para facilitar la lectura. */
                /* Si no hay productos devuelve lista vacía sin lanzar error */
                return productoDAO.GetAll()
                                  .OrderBy(p => p.Modelo)
                                  .ThenBy(p => p.Talle)
                                  .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el estado del stock: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: ObtenerCambiosPorPeriodo
         * Descripción: Obtiene los cambios de calzado realizados en un período (devoluciones CU12).
         * Parámetros: fecha desde y fecha hasta del período.
         * Retorna: lista de cambios registrados. Vacía si no hay cambios.
         -----------------------------------------------------------------------------------------------------*/
        public List<CambioCalzado> ObtenerCambiosPorPeriodo(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                /* Se valida que el período sea correcto antes de consultar */
                ValidarPeriodo(fechaDesde, fechaHasta);

                /* Se obtiene los cambios registrados dentro del período indicado */
                /* Si no hay cambios devuelve lista vacía sin lanzar error */
                return cambioDAO.GetByPeriod(fechaDesde, fechaHasta);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los cambios por período: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: ObtenerTotalVentas
         * Descripción: Calcula el total de ventas en un período para el dashboard.
         * Parámetros: fecha desde y fecha hasta del período.
         * Retorna: total de ventas en el período. Cero si no hay ventas.
         -----------------------------------------------------------------------------------------------------*/
        public decimal ObtenerTotalVentas(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                /* Se valida que el período sea correcto antes de consultar */
                ValidarPeriodo(fechaDesde, fechaHasta);

                /* Se obtiene todas las ventas del período */
                List<Venta> ventas = ventaDAO.GetByPeriod(fechaDesde, fechaHasta);

                /* Se inicializa la variable acumuladora. */
                decimal total = 0;

                /* Se suma el total de cada venta */
                foreach (Venta v in ventas)
                    total += v.Total;

                /* Se devuelve el total acumulado. Si no hubo ventas devuelve cero */
                return total;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el total de ventas: " + ex.Message);
            }
        }


         /* -----------------------------------------------------------------------------------------------------
         * FUNCIÓN PRIVADA
         -----------------------------------------------------------------------------------------------------*/
        /* -----------------------------------------------------------------------------------------------------
         * Función: ValidarPeriodo
         * Descripción: Centraliza las validaciones del período para todos los métodos de reporte.
         *              Se llama antes de cualquier consulta que reciba fechaDesde y fechaHasta.
         * Parámetros: fecha desde y fecha hasta del período.
         -----------------------------------------------------------------------------------------------------*/
        private void ValidarPeriodo(DateTime fechaDesde, DateTime fechaHasta)
        {
            /* La fecha desde no puede ser mayor a la fecha hasta porque no tendría sentido */
            if (fechaDesde > fechaHasta)
                throw new Exception("La fecha desde no puede ser mayor a la fecha hasta.");

            /* La fecha hasta no puede ser futura porque no hay datos de esas fechas todavía */
            if (fechaHasta.Date > DateTime.Today)
                throw new Exception("La fecha hasta no puede ser una fecha futura.");

            /* La fecha desde no puede ser más de 10 años atrás para evitar búsquedas irreales */
            if (fechaDesde.Date < DateTime.Today.AddYears(-10))
                throw new Exception("La fecha desde no puede ser anterior a 10 años atrás.");
        }
    }
}
