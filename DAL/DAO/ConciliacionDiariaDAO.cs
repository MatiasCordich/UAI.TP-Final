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
     * Clase: ConciliacionDiariaDAO
     * Descripción: Capa de acceso a datos para la entidad ConciliacionDiaria.
     *              Utiliza ConciliacionDiariaORM para realizar las operaciones de persistencia en el XML.
     *              Se genera al cierre de jornada para verificar pagos vs egresos (CU08).
     -----------------------------------------------------------------------------------------------------*/
    public class ConciliacionDiariaDAO
    {
        private ConciliacionDiariaORM orm = new ConciliacionDiariaORM();

        public List<ConciliacionDiaria> GetAll()
        {
            try
            {
                return orm.ObtenerTodos();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las conciliaciones: " + ex.Message);
            }
        }

        public ConciliacionDiaria GetById(int id)
        {
            try
            {
                return orm.ObtenerPorId(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la conciliación: " + ex.Message);
            }
        }

        /* Conciliación de una fecha específica (CU08) */
        public ConciliacionDiaria GetByDate(DateTime fecha)
        {
            try
            {
                return orm.ObtenerTodos()
                          .FirstOrDefault(c => c.Fecha.Date == fecha.Date);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la conciliación por fecha: " + ex.Message);
            }
        }

        /* Conciliaciones con alerta para revisión del encargado (CU08) */
        public List<ConciliacionDiaria> GetWithAlert()
        {
            try
            {
                return orm.ObtenerTodos()
                          .Where(c => c.TieneAlerta)
                          .OrderByDescending(c => c.Fecha)
                          .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las conciliaciones con alerta: " + ex.Message);
            }
        }

        public void Insert(ConciliacionDiaria conciliacion)
        {
            try
            {
                orm.Insertar(conciliacion);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar la conciliación: " + ex.Message);
            }
        }

        public void Update(ConciliacionDiaria conciliacion)
        {
            try
            {
                orm.Actualizar(conciliacion);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la conciliación: " + ex.Message);
            }
        }
    }
}
