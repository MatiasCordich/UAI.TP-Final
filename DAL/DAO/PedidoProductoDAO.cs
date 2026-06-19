using DAL.ORM;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAO
{
    /* -----------------------------------------------------------------------------------------------------
     * Clase: PedidoProductoDAO
     * Descripción: Capa de acceso a datos para la entidad PedidoProducto.
     *              Utiliza PedidoProductoORM para realizar las operaciones de persistencia en el XML.
     *              Representa el detalle de productos de cada pedido de compra.
     *              Incluye la cantidad recibida y observaciones para el cotejo del remito (CU05).
     -----------------------------------------------------------------------------------------------------*/
    public class PedidoProductoDAO
    {
        private PedidoProductoORM orm = new PedidoProductoORM();

        public List<PedidoProducto> GetAll()
        {
            try
            {
                return orm.ObtenerTodos();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los detalles de pedido: " + ex.Message);
            }
        }

        /* Detalle de productos de un pedido específico (CU05) */
        public List<PedidoProducto> GetByOrder(int idPedido)
        {
            try
            {
                return orm.ObtenerTodos()
                          .Where(pp => pp.IdPedido == idPedido)
                          .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el detalle del pedido: " + ex.Message);
            }
        }

        public void Insert(PedidoProducto pedidoProducto)
        {
            try
            {
                orm.Insertar(pedidoProducto);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el detalle del pedido: " + ex.Message);
            }
        }

        public void Update(PedidoProducto pedidoProducto)
        {
            try
            {
                orm.Actualizar(pedidoProducto);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el detalle del pedido: " + ex.Message);
            }
        }
    }
}
