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
     * Clase: AlertaStockDAO
     * Descripción: Capa de acceso a datos para la entidad AlertaStock.
     *              Utiliza AlertaStockORM para realizar las operaciones de persistencia en el XML.
     *              Las alertas persisten hasta que el encargado las resuelva (CU10).
     -----------------------------------------------------------------------------------------------------*/
    public class AlertaStockDAO
    {
        private AlertaStockORM orm = new AlertaStockORM();

        public List<AlertaStock> GetAll()
        {
            try
            {
                return orm.ObtenerTodos();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las alertas de stock: " + ex.Message);
            }
        }

        public AlertaStock GetById(int id)
        {
            try
            {
                return orm.ObtenerPorId(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la alerta de stock: " + ex.Message);
            }
        }

        /* Alertas no resueltas para notificar al encargado de compras (CU10) */
        public List<AlertaStock> GetPending()
        {
            try
            {
                return orm.ObtenerTodos()
                          .Where(a => !a.Resuelta)
                          .OrderByDescending(a => a.Fecha)
                          .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las alertas pendientes: " + ex.Message);
            }
        }

        public void Insert(AlertaStock alerta)
        {
            try
            {
                orm.Insertar(alerta);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar la alerta de stock: " + ex.Message);
            }
        }

        public void Update(AlertaStock alerta)
        {
            try
            {
                orm.Actualizar(alerta);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la alerta de stock: " + ex.Message);
            }
        }
    }
}
