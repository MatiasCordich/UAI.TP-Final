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
     * Clase: CuponFidelizacionDAO
     * Descripción: Capa de acceso a datos para la entidad CuponFidelizacion.
     *              Utiliza CuponFidelizacionORM para realizar las operaciones de persistencia en el XML.
     *              Los cupones se generan por cumpleaños o historial de compras (CU11).
     -----------------------------------------------------------------------------------------------------*/
    public class CuponFidelizacionDAO
    {
        private CuponFidelizacionORM orm = new CuponFidelizacionORM();

        public List<CuponFidelizacion> GetAll()
        {
            try
            {
                return orm.ObtenerTodos();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los cupones: " + ex.Message);
            }
        }

        public CuponFidelizacion GetById(int id)
        {
            try
            {
                return orm.ObtenerPorId(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el cupón: " + ex.Message);
            }
        }

        /* Cupones vigentes no usados de un cliente (CU11) */
        public List<CuponFidelizacion> GetByCustomer(int idCliente)
        {
            try
            {
                return orm.ObtenerTodos()
                          .Where(c => c.IdCliente == idCliente
                                   && !c.Usado
                                   && c.FechaVencimiento >= DateTime.Today)
                          .OrderByDescending(c => c.FechaEmision)
                          .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los cupones del cliente: " + ex.Message);
            }
        }

        public void Insert(CuponFidelizacion cupon)
        {
            try
            {
                orm.Insertar(cupon);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el cupón: " + ex.Message);
            }
        }

        public void Update(CuponFidelizacion cupon)
        {
            try
            {
                orm.Actualizar(cupon);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el cupón: " + ex.Message);
            }
        }
    }
}
