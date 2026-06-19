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
     * Clase: VentaDAO
     * Descripción: Capa de acceso a datos para la entidad Venta.
     *              Utiliza VentaORM para realizar las operaciones de persistencia en el archivo XML.
     *              Incluye métodos para buscar ventas por cliente (CU03) y por período (dashboard CU12).
     -----------------------------------------------------------------------------------------------------*/
    public class VentaDAO
    {
        private VentaORM orm = new VentaORM();

        public List<Venta> GetAll()
        {
            try
            {
                return orm.ObtenerTodos();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las ventas: " + ex.Message);
            }
        }

        public Venta GetById(int id)
        {
            try
            {
                return orm.ObtenerPorId(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la venta: " + ex.Message);
            }
        }

        /* Ventas de un cliente específico (CU03) */
        public List<Venta> GetByCustomer(int idCliente)
        {
            try
            {
                return orm.ObtenerTodos()
                          .Where(v => v.IdCliente == idCliente)
                          .OrderByDescending(v => v.Fecha)
                          .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las ventas del cliente: " + ex.Message);
            }
        }

        /* Ventas filtradas por período para el dashboard (CU12) */
        public List<Venta> GetByPeriod(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                return orm.ObtenerTodos()
                          .Where(v => v.Fecha >= fechaDesde && v.Fecha <= fechaHasta)
                          .OrderByDescending(v => v.Fecha)
                          .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las ventas por período: " + ex.Message);
            }
        }

        public void Insert(Venta venta)
        {
            try
            {
                orm.Insertar(venta);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar la venta: " + ex.Message);
            }
        }

        public void Update(Venta venta)
        {
            try
            {
                orm.Actualizar(venta);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la venta: " + ex.Message);
            }
        }
    }
}
