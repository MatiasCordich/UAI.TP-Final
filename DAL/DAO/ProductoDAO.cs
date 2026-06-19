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
     * Clase: ProductoDAO
     * Descripción: Capa de acceso a datos para la entidad Producto.
     *              Utiliza ProductoORM para realizar las operaciones de persistencia en el archivo XML.
     *              Incluye métodos específicos para buscar por modelo/talle (CU01) y
     *              obtener productos con stock crítico (CU10).
     -----------------------------------------------------------------------------------------------------*/
    public class ProductoDAO
    {
        private ProductoORM orm = new ProductoORM();

        public List<Producto> GetAll()
        {
            try
            {
                return orm.ObtenerTodos().Where(p => p.Activo).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los productos: " + ex.Message);
            }
        }

        public Producto GetById(int id)
        {
            try
            {
                return orm.ObtenerPorId(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el producto: " + ex.Message);
            }
        }

        /* Busca productos por modelo y talle con stock disponible (CU01) */
        public List<Producto> GetByModelAndSize(string modelo, int talle)
        {
            try
            {
                return orm.ObtenerTodos()
                          .Where(p => p.Activo
                                   && p.Modelo == modelo
                                   && p.Talle == talle
                                   && p.Stock > 0)
                          .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el producto por modelo y talle: " + ex.Message);
            }
        }

        /* Productos con stock igual o menor al mínimo configurado (CU10) */
        public List<Producto> GetCriticalStock()
        {
            try
            {
                return orm.ObtenerTodos()
                          .Where(p => p.Activo && p.Stock <= p.StockMinimo)
                          .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener productos con stock crítico: " + ex.Message);
            }
        }

        public void Insert(Producto producto)
        {
            try
            {
                orm.Insertar(producto);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el producto: " + ex.Message);
            }
        }

        public void Update(Producto producto)
        {
            try
            {
                orm.Actualizar(producto);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el producto: " + ex.Message);
            }
        }

        /* Baja lógica: no se elimina, se desactiva */
        public void Deactivate(int id)
        {
            try
            {
                Producto producto = orm.ObtenerPorId(id);
                producto.Activo = false;
                orm.Actualizar(producto);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al desactivar el producto: " + ex.Message);
            }
        }
    }
}
