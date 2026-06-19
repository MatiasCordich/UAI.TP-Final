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
     * Clase: AjusteStockDAO
     * Descripción: Capa de acceso a datos para la entidad AjusteStock.
     *              Utiliza AjusteStockORM para realizar las operaciones de persistencia en el XML.
     *              Registra las bajas de stock por merma o rotura con trazabilidad (CU09).
     -----------------------------------------------------------------------------------------------------*/
    public class AjusteStockDAO
    {
        private AjusteStockORM orm = new AjusteStockORM();

        public List<AjusteStock> GetAll()
        {
            try
            {
                return orm.ObtenerTodos();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los ajustes de stock: " + ex.Message);
            }
        }

        public AjusteStock GetById(int id)
        {
            try
            {
                return orm.ObtenerPorId(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el ajuste de stock: " + ex.Message);
            }
        }

        /* Historial de ajustes de un producto específico (CU09) */
        public List<AjusteStock> GetByProduct(int idProducto)
        {
            try
            {
                return orm.ObtenerTodos()
                          .Where(a => a.IdProducto == idProducto)
                          .OrderByDescending(a => a.Fecha)
                          .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los ajustes del producto: " + ex.Message);
            }
        }

        public void Insert(AjusteStock ajuste)
        {
            try
            {
                orm.Insertar(ajuste);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el ajuste de stock: " + ex.Message);
            }
        }
    }
}
