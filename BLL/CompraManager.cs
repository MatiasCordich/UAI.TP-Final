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
     * Clase: CompraManager
     * Descripción: Capa de lógica de negocio para la gestión de compras.
     *              Gestiona la generación de pedidos de reposición y la recepción de mercadería.
     -----------------------------------------------------------------------------------------------------*/
    public class CompraManager
    {
        private PedidoCompraDAO pedidoDAO = new PedidoCompraDAO();
        private PedidoProductoDAO pedidoProductoDAO = new PedidoProductoDAO();
        private ProductoManager productoManager = new ProductoManager();
        private StockManager stockManager = new StockManager();

        /* Estados válidos del pedido */
        private static readonly List<string> EstadosValidos = new List<string>
        {
            "Pendiente", "Confirmado", "Recibido", "Cancelado"
        };

        /* -----------------------------------------------------------------------------------------------------
         * Función: GenerarPedidoReposicion
         * Descripción: Genera un pedido de reposición cuando el stock llega al mínimo (CU04).
         *              Los pedidos se agrupan por proveedor.
         * Parámetros: ID del proveedor y detalle de productos a reponer.
         -----------------------------------------------------------------------------------------------------*/
        public void GenerarPedidoReposicion(int idProveedor, List<PedidoProducto> detalle)
        {
            try
            {
                /* Se valida que el pedido tenga un proveedor asignado. */
                if (idProveedor <= 0)
                    throw new Exception("Debe asignar un proveedor al pedido.");

                /* Se validan los datos de los detalle del pedido. */
                ValidarDetallePedido(detalle);

                /* Se crea el pedido con estado Pendiente */
                PedidoCompra pedido = new PedidoCompra
                {
                    IdProveedor = idProveedor,
                    Fecha = DateTime.Now,
                    Estado = "Pendiente"
                };

                /* Se inserta el pedido en el XML. */
                pedidoDAO.Insert(pedido);

                /* Se guarda cada detale del pedido en el XML. */
                foreach (PedidoProducto pp in detalle)
                {
                    /* Asocia el detalle al pedido recién creado */
                    pp.IdPedido = pedido.Id;
                    pp.CantidadRecibida = 0;
                    pedidoProductoDAO.Insert(pp);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar el pedido de reposición: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: RecepcionarMercaderia
         * Descripción: Registra la recepción de mercadería cotejando el remito contra el pedido (CU05).
         *              Actualiza el stock de los productos recibidos y marca el pedido como recibido.
         * Parámetros: ID del pedido y detalle de cantidades recibidas con observaciones.
         -----------------------------------------------------------------------------------------------------*/
        public void RecepcionarMercaderia(int idPedido, List<PedidoProducto> detalleRecibido)
        {
            try
            {
                /* Se valida que el pedido exista y esté en estado Pendiente */
                PedidoCompra pedido = pedidoDAO.GetById(idPedido);
                if (pedido == null)
                    throw new Exception("El pedido no existe.");

                if (pedido.Estado != "Pendiente")
                    throw new Exception("El pedido no está en estado Pendiente.");

                /* Se valida que el detalle recibido no esté vacío */
                if (detalleRecibido == null || detalleRecibido.Count == 0)
                    throw new Exception("Debe ingresar al menos un producto recibido.");

                /* Se actualiza el detalle con las cantidades efectivamente recibidas. */
                foreach (PedidoProducto ppRecibido in detalleRecibido)
                {
                    /* Se valida que la cantidad recibida no sea negativa */
                    if (ppRecibido.CantidadRecibida < 0)
                        throw new Exception("La cantidad recibida no puede ser negativa.");

                    /* Se busca el detalle original del pedido para ese producto */
                    List<PedidoProducto> detalleOriginal = pedidoProductoDAO.GetByOrder(idPedido);
                    PedidoProducto ppOriginal = detalleOriginal.Find(p => p.IdProducto == ppRecibido.IdProducto);

                    if (ppOriginal != null)
                    {
                        /* Si hay diferencia entre lo pedido y lo recibido, la observación es obligatoria */
                        if (ppRecibido.CantidadRecibida != ppOriginal.Cantidad &&
                            string.IsNullOrEmpty(ppRecibido.Observacion))
                            throw new Exception($"Hay diferencia en el producto ID {ppRecibido.IdProducto}. Debe ingresar una observación.");

                        /* Se actualiza la cantidad recibida y la observación en el XML */
                        ppOriginal.CantidadRecibida = ppRecibido.CantidadRecibida;
                        ppOriginal.Observacion = ppRecibido.Observacion;
                        pedidoProductoDAO.Update(ppOriginal);
                    }

                    /* Suma el stock recibido al producto si se recibió algo. */
                    if (ppRecibido.CantidadRecibida > 0)
                    {
                        /* Se obtiene el producto. */
                        Producto producto = productoManager.ObtenerProducto(ppRecibido.IdProducto);

                        /* Se ajusta el stock del producto. */
                        productoManager.AjustarStock(producto, ppRecibido.CantidadRecibida);

                        /* Se obtienen las alertas pendientes. */
                        List<AlertaStock> alertas = new AlertaStockDAO().GetPending();

                        /* Si hay alerta de stock de ese producto, se resuelve el alerta. */
                        AlertaStock alerta = alertas.Find(a => a.IdProducto == ppRecibido.IdProducto);
                        if (alerta != null)
                            stockManager.ResolverAlerta(alerta.Id);
                    }
                }

                /* Se marca el pedido como Recibido */
                pedido.Estado = "Recibido";

                /* Se llama al DAO de pedidos para modificar el estado del pedido.*/
                pedidoDAO.Update(pedido);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al recepcionar la mercadería: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: ObtenerPedidosPendientes
         * Descripción: Obtiene todos los pedidos en estado Pendiente para el módulo de recepción.
         * Retorna: lista de pedidos pendientes.
         -----------------------------------------------------------------------------------------------------*/
        public List<PedidoCompra> ObtenerPedidosPendientes()
        {
            try
            {
                /* Filtra los pedidos en estado Pendiente */
                return pedidoDAO.GetPending();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los pedidos pendientes: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: CancelarPedido
         * Descripción: Cancela un pedido de compra pendiente.
         * Parámetros: ID del pedido a cancelar.
         -----------------------------------------------------------------------------------------------------*/
        public void CancelarPedido(int idPedido)
        {
            try
            {
                /* Verifica que el pedido exista */
                PedidoCompra pedido = pedidoDAO.GetById(idPedido);
                if (pedido == null)
                    throw new Exception("El pedido no existe.");

                /* Solo se pueden cancelar pedidos en estado Pendiente */
                if (pedido.Estado != "Pendiente")
                    throw new Exception("Solo se pueden cancelar pedidos en estado Pendiente.");

                /* Cambia el estado a Cancelado */
                pedido.Estado = "Cancelado";

                /* Se llama el DAO de pedidos para realizar la modificación en el XML. */
                pedidoDAO.Update(pedido);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cancelar el pedido: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * FUNCIÓN PRIVADA
         -----------------------------------------------------------------------------------------------------*/

        /* -----------------------------------------------------------------------------------------------------
         * Función: ValidarDetallePedido
         * Descripción: Centraliza las validaciones del detalle de un pedido de compra.
         *              Se llama en GenerarPedidoReposicion antes de insertar.
         * Parámetros: detalle de productos del pedido.
         -----------------------------------------------------------------------------------------------------*/
        private void ValidarDetallePedido(List<PedidoProducto> detalle)
        {
            /* Valida que el detalle no esté vacío */
            if (detalle == null || detalle.Count == 0)
                throw new Exception("El pedido debe tener al menos un producto.");

            /* Lista para detectar productos duplicados en el detalle (lista de IDs de productos.) */
            List<int> idsProductos = new List<int>();

            /* Por cada pedido del detalle se hace lo siguiente*/
            foreach (PedidoProducto pp in detalle)
            {
                /* Se valida que cada línea tenga un producto asignado */
                if (pp.IdProducto <= 0)
                    throw new Exception("Cada línea del pedido debe tener un producto asignado.");

                /* Se valida que la cantidad a pedir sea mayor a cero */
                if (pp.Cantidad <= 0)
                    throw new Exception("La cantidad de cada producto debe ser mayor a cero.");

                /* Se valida que no se repita el mismo producto en el detalle */
                if (idsProductos.Contains(pp.IdProducto))
                    throw new Exception($"El producto ID {pp.IdProducto} está duplicado en el pedido.");

                /* Si el id no está duplicado, se agrega a la lista y se sigue iterando según el detalle del pedido. */
                idsProductos.Add(pp.IdProducto);
            }
        }
    }
}
