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
     * Clase: VentaProductoDAO
     * Descripción: Capa de acceso a datos para la entidad VentaProducto.
     *              Utiliza VentaProductoORM para realizar las operaciones de persistencia en el XML.
     *              Representa el detalle de productos de cada venta.
     -----------------------------------------------------------------------------------------------------*/
    public class VentaProductoDAO
    {
        private VentaProductoORM orm = new VentaProductoORM();

        public List<VentaProducto> GetAll()
        {
            try
            {
                return orm.ObtenerTodos();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los detalles de venta: " + ex.Message);
            }
        }

        /* Detalle de productos de una venta específica */
        public List<VentaProducto> GetBySale(int idVenta)
        {
            try
            {
                return orm.ObtenerTodos()
                          .Where(vp => vp.IdVenta == idVenta)
                          .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el detalle de la venta: " + ex.Message);
            }
        }

        public void Insert(VentaProducto ventaProducto)
        {
            try
            {
                orm.Insertar(ventaProducto);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el detalle de venta: " + ex.Message);
            }
        }

        public void Update(VentaProducto ventaProducto)
        {
            try
            {
                orm.Actualizar(ventaProducto);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el detalle de venta: " + ex.Message);
            }
        }
    }
}
