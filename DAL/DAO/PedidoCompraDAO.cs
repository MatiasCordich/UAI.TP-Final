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
     * Clase: PedidoCompraDAO
     * Descripción: Capa de acceso a datos para la entidad PedidoCompra.
     *              Utiliza PedidoCompraORM para realizar las operaciones de persistencia en el XML.
     *              Incluye métodos para obtener pedidos pendientes (CU05) y por proveedor.
     -----------------------------------------------------------------------------------------------------*/
    public class PedidoCompraDAO
    {
        private PedidoCompraORM orm = new PedidoCompraORM();

        public List<PedidoCompra> GetAll()
        {
            try
            {
                return orm.ObtenerTodos();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los pedidos de compra: " + ex.Message);
            }
        }

        public PedidoCompra GetById(int id)
        {
            try
            {
                return orm.ObtenerPorId(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el pedido de compra: " + ex.Message);
            }
        }

        /* Pedidos en estado Pendiente para el módulo de recepción (CU05) */
        public List<PedidoCompra> GetPending()
        {
            try
            {
                return orm.ObtenerTodos()
                          .Where(p => p.Estado == "Pendiente")
                          .OrderByDescending(p => p.Fecha)
                          .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los pedidos pendientes: " + ex.Message);
            }
        }

        /* Pedidos de un proveedor específico */
        public List<PedidoCompra> GetBySupplier(int idProveedor)
        {
            try
            {
                return orm.ObtenerTodos()
                          .Where(p => p.IdProveedor == idProveedor)
                          .OrderByDescending(p => p.Fecha)
                          .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los pedidos del proveedor: " + ex.Message);
            }
        }

        public void Insert(PedidoCompra pedido)
        {
            try
            {
                orm.Insertar(pedido);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el pedido de compra: " + ex.Message);
            }
        }

        public void Update(PedidoCompra pedido)
        {
            try
            {
                orm.Actualizar(pedido);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el pedido de compra: " + ex.Message);
            }
        }

        public void Delete(int id)
        {
            try
            {
                orm.Eliminar(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el pedido de compra: " + ex.Message);
            }
        }
    }
}
