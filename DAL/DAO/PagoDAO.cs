using DAL.ORM;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;


namespace DAL.DAO
{
    /* -----------------------------------------------------------------------------------------------------
     * Clase: PagoDAO
     * Descripción: Capa de acceso a datos para la entidad Pago.
     *              Utiliza PagoORM para realizar las operaciones de persistencia en el archivo XML.
     *              Incluye métodos para obtener pagos por proveedor (CU06) y por fecha (CU08).
     -----------------------------------------------------------------------------------------------------*/
    public class PagoDAO
    {
        private PagoORM orm = new PagoORM();

        public List<Pago> GetAll()
        {
            try
            {
                return orm.ObtenerTodos();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los pagos: " + ex.Message);
            }
        }

        public Pago GetById(int id)
        {
            try
            {
                return orm.ObtenerPorId(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el pago: " + ex.Message);
            }
        }

        /* Pagos de un proveedor específico (CU06) */
        public List<Pago> GetBySupplier(int idProveedor)
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
                throw new Exception("Error al obtener los pagos del proveedor: " + ex.Message);
            }
        }

        /* Pagos de una fecha específica para la conciliación diaria (CU08) */
        public List<Pago> GetByDate(DateTime fecha)
        {
            try
            {
                return orm.ObtenerTodos()
                          .Where(p => p.Fecha.Date == fecha.Date)
                          .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los pagos por fecha: " + ex.Message);
            }
        }

        public void Insert(Pago pago)
        {
            try
            {
                orm.Insertar(pago);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el pago: " + ex.Message);
            }
        }

        public void Update(Pago pago)
        {
            try
            {
                orm.Actualizar(pago);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el pago: " + ex.Message);
            }
        }
    }
}
