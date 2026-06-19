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
     * Clase: StockManager
     * Descripción: Capa de lógica de negocio para el control de stock.
     *              Gestiona los ajustes por merma/rotura y las alertas de stock crítico.
     -----------------------------------------------------------------------------------------------------*/
    public class StockManager
    {
        private AjusteStockDAO ajusteStockDAO = new AjusteStockDAO();
        private AlertaStockDAO alertaStockDAO = new AlertaStockDAO();
        private ProductoDAO productoDAO = new ProductoDAO();

        /* -----------------------------------------------------------------------------------------------------
         * Función: RegistrarAjuste
         * Descripción: Registra una baja de stock por merma o rotura con trazabilidad (CU09).
         *              Descuenta el stock del producto y verifica si llegó al mínimo.
         * Parámetros: ID del producto, cantidad a dar de baja y motivo.
         -----------------------------------------------------------------------------------------------------*/
        public void RegistrarAjuste(int idProducto, int cantidad, string motivo)
        {
            try
            {
                /* Obligatoriedad del motivo de ajuste. */
                if (string.IsNullOrEmpty(motivo))
                    throw new Exception("Debe ingresar un motivo para el ajuste.");

                /* La cantidad a ajustar no puede ser menor o igual a cero. */
                if (cantidad <= 0)
                    throw new Exception("La cantidad debe ser mayor a cero.");

                /* Verifica que el producto exista */
                Producto producto = productoDAO.GetById(idProducto);
                if (producto == null)
                    throw new Exception("El producto no existe.");

                /* Verifica que haya suficiente stock para realizar el ajuste */
                if (producto.Stock < cantidad)
                    throw new Exception("No hay suficiente stock para realizar el ajuste.");

                /* Guarda el ajuste con trazabilidad (cantidad negativa = baja) */
                AjusteStock ajuste = new AjusteStock
                {
                    IdProducto = idProducto,
                    Cantidad = -cantidad,
                    Motivo = motivo,
                    Fecha = DateTime.Now
                };

                /* Llama al DAO para insertar el registro de ajuste de stock. */
                ajusteStockDAO.Insert(ajuste);

                /* Descuenta el stock del producto en el XML */
                producto.Stock -= cantidad;

                /* Modifica los datos (stock) del producto. */
                productoDAO.Update(producto);

                /* Verifica si con el ajuste, el stock de ese producto llegó al mínimo para emitir alerta (CU10) */
                if (producto.Stock <= producto.StockMinimo)
                    EmitirAlerta(idProducto);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el ajuste de stock: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: EmitirAlerta
         * Descripción: Genera una alerta de stock crítico que persiste hasta resolverse (CU10).
         * Parámetros: ID del producto con stock crítico.
         -----------------------------------------------------------------------------------------------------*/
        public void EmitirAlerta(int idProducto)
        {
            try
            {
                /* Verifica si ya existe una alerta pendiente para evitar duplicados */
                List<AlertaStock> alertasPendientes = alertaStockDAO.GetPending();
                bool yaExiste = alertasPendientes.Exists(a => a.IdProducto == idProducto);

                /* Solo genera la alerta si no existe una pendiente para ese producto */
                if (!yaExiste)
                {
                    AlertaStock alerta = new AlertaStock
                    {
                        IdProducto = idProducto,
                        Fecha = DateTime.Now,
                        Resuelta = false
                    };

                    /* Lllama al DAO para insertar la nueva alarta al XML.*/
                    alertaStockDAO.Insert(alerta);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al emitir la alerta de stock: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: ResolverAlerta
         * Descripción: Marca una alerta como resuelta cuando se genera la reposición.
         * Parámetros: ID de la alerta.
         -----------------------------------------------------------------------------------------------------*/
        public void ResolverAlerta(int idAlerta)
        {
            try
            {
                /* Busca la alerta y la marca como resuelta */
                AlertaStock alerta = alertaStockDAO.GetById(idAlerta);
                if (alerta == null)
                    throw new Exception("La alerta no existe.");

                /* Se marca la alerta como resuelta. */
                alerta.Resuelta = true;

                /* Se llama al DAO de alerta para modificar el estado de la misma. */
                alertaStockDAO.Update(alerta);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al resolver la alerta: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: ObtenerAlertasPendientes
         * Descripción: Obtiene todas las alertas no resueltas para el encargado de compras.
         * Retorna: lista de alertas pendientes.
         -----------------------------------------------------------------------------------------------------*/
        public List<AlertaStock> ObtenerAlertasPendientes()
        {
            try
            {
                /* Filtra solo las alertas no resueltas */
                return alertaStockDAO.GetPending();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las alertas pendientes: " + ex.Message);
            }
        }
    }
}
