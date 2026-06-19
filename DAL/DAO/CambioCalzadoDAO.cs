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
     * Clase: CambioCalzadoDAO
     * Descripción: Capa de acceso a datos para la entidad CambioCalzado.
     *              Utiliza CambioCalzadoORM para realizar las operaciones de persistencia en el XML.
     *              Registra los cambios de calzado en postventa (CU03).
     -----------------------------------------------------------------------------------------------------*/
    public class CambioCalzadoDAO
    {
        private CambioCalzadoORM orm = new CambioCalzadoORM();

        public List<CambioCalzado> GetAll()
        {
            try
            {
                return orm.ObtenerTodos();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los cambios de calzado: " + ex.Message);
            }
        }

        public CambioCalzado GetById(int id)
        {
            try
            {
                return orm.ObtenerPorId(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el cambio de calzado: " + ex.Message);
            }
        }

        /* Cambios asociados a una venta original (CU03) */
        public List<CambioCalzado> GetBySale(int idVentaOriginal)
        {
            try
            {
                return orm.ObtenerTodos()
                          .Where(c => c.IdVentaOriginal == idVentaOriginal)
                          .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los cambios de la venta: " + ex.Message);
            }
        }

        /* Cambios por período para el dashboard (CU12) */
        public List<CambioCalzado> GetByPeriod(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                return orm.ObtenerTodos()
                          .Where(c => c.Fecha >= fechaDesde && c.Fecha <= fechaHasta)
                          .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los cambios por período: " + ex.Message);
            }
        }

        public void Insert(CambioCalzado cambio)
        {
            try
            {
                orm.Insertar(cambio);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el cambio de calzado: " + ex.Message);
            }
        }
    }
}
