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

        /* -----------------------------------------------------------------------------------------------------
        * Función: GetAllOrFiltered
        * Descripción: Obtiene TODOS los productos.
        *              A su vez tiene parámetros opcionales si se quiere filtrar la búsqueda.
        * Parámetros:
        *              modelo → filtra por modelo (contiene)
        *              activo → filtra por estado (true=activo, false=inactivo, null=todos)
        -----------------------------------------------------------------------------------------------------*/
        public List<Producto> GetAllOrFiltered(string modelo = null, bool? activo = null)
        {
            try
            {
                /* Obtiene todos los productos del XML sin filtrar */
                List<Producto> productos = orm.ObtenerTodos();

                /* Si se proporcionó un modelo, se aplica el filtro */
                if (!string.IsNullOrEmpty(modelo))
                {
                    productos = productos.Where(
                        p => p.Modelo.ToUpper()
                        .Contains(modelo.ToUpper())).ToList();
                }

                /* Si se proporcionó un estado, se aplica el filtro */
                if (activo.HasValue)
                {
                    productos = productos.Where(p => p.Activo == activo.Value).ToList();
                }

                /* Devuelve los productos */
                return productos;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los productos: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: GetById
         * Descripción: Obtiene un producto por su ID.
         * Parámetros: id → id del producto.
         -----------------------------------------------------------------------------------------------------*/
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

        /* -----------------------------------------------------------------------------------------------------
         * Función: GetByModelAndSize
         * Descripción: Busca productos activos por modelo y talle - usado en ventas (CU01).
         * Parámetros: modelo y talle del calzado.
         -----------------------------------------------------------------------------------------------------*/
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

        /* -----------------------------------------------------------------------------------------------------
         * Función: Insert
         * Descripción: Registra el producto.
         * Parámetros: La entidad Producto.
         -----------------------------------------------------------------------------------------------------*/
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

        /* -----------------------------------------------------------------------------------------------------
         * Función: Update
         * Descripción: Modifica el producto.
         * Parámetros: La entidad Producto.
         -----------------------------------------------------------------------------------------------------*/
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

        /* -----------------------------------------------------------------------------------------------------
         * Función: Deactivate
         * Descripción: Baja lógica del producto.
         * Parámetros: El id del producto a desactivar.
         -----------------------------------------------------------------------------------------------------*/
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
