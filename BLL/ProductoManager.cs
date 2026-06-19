using DAL.DAO;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    /* -----------------------------------------------------------------------------------------------------
     * Clase: ProductoManager
     * Descripción: Capa de lógica de negocio para la entidad Producto.
     *              Gestiona el alta, baja, modificación y consulta de productos.
     *              Incluye lógica para buscar por modelo/talle (CU01) y detectar
     *              stock crítico para emitir alertas (CU10).
     -----------------------------------------------------------------------------------------------------*/
    public class ProductoManager
    {
        private ProductoDAO productoDAO = new ProductoDAO();
        private StockManager stockManager = new StockManager();

        /* -----------------------------------------------------------------------------------------------------
         * Función: ListarProductos
         * Descripción: Obtiene todos los productos activos.
         * Retorna: lista de productos.
         -----------------------------------------------------------------------------------------------------*/
        public List<Producto> ListarProductos()
        {
            try
            {
                /* Obtiene todos los productos activos del XML */
                return productoDAO.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los productos: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: ObtenerProducto
         * Descripción: Obtiene un producto por su ID.
         * Parámetros: ID del producto.
         * Retorna: producto encontrado.
         -----------------------------------------------------------------------------------------------------*/
        public Producto ObtenerProducto(int id)
        {
            try
            {
                /* Se busca el producto por ID */
                Producto producto = productoDAO.GetById(id);

                /* Se valida la existencia de dicho producto */
                if (producto == null)
                    throw new Exception("El producto no existe.");

                /* Se devuelve el producto */
                return producto;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el producto: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: BuscarPorModeloYTalle
         * Descripción: Busca productos disponibles por modelo y talle (CU01).
         * Parámetros: modelo y talle del calzado.
         * Retorna: lista de productos disponibles.
         -----------------------------------------------------------------------------------------------------*/
        public List<Producto> BuscarPorModeloYTalle(string modelo, int talle)
        {
            try
            {
                /* Se buscan productos con stock disponible para el modelo y talle indicados */
                List<Producto> productos = productoDAO.GetByModelAndSize(modelo, talle);

                /* Se verifica si hay stock de esos productos según modelo y talle */
                if (productos.Count == 0)
                    throw new Exception("No hay stock disponible para ese modelo y talle.");

                /* Se devuelven los productos encontrados */
                return productos;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar el producto: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: RegistrarProducto
         * Descripción: Da de alta un nuevo producto validando las reglas de negocio.
         * Parámetros: producto a registrar.
         -----------------------------------------------------------------------------------------------------*/
        public void RegistrarProducto(Producto producto)
        {
            try
            {
                /* Se realizan todas las validaciones correspondientes antes de registrar el producto. */
                ValidarProducto(producto);

                /* Se setea el producto con estado activo = true */
                producto.Activo = true;

                /* Se inserta el producto en el XML */
                productoDAO.Insert(producto);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el producto: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: ModificarProducto
         * Descripción: Modifica los datos de un producto validando las reglas de negocio.
         * Parámetros: producto con datos actualizados.
         -----------------------------------------------------------------------------------------------------*/
        public void ModificarProducto(Producto producto)
        {
            try
            {
                /* Verifica que el producto exista y esté activo */
                Producto existente = productoDAO.GetById(producto.Id);
                if (existente == null)
                    throw new Exception("El producto no existe.");
                if (!existente.Activo)
                    throw new Exception("No se puede modificar un producto desactivado.");

                /* Se realizan todas las validaciones correspondientes antes de realizar la modificación. */
                ValidarProducto(producto);

                /* Actualiza el producto en el XML */
                productoDAO.Update(producto);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar el producto: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: AjustarStock
         * Descripción: Descuenta o suma stock a un producto y verifica si llegó al mínimo.
         *              Si llega al mínimo, genera una alerta automática (CU10).
         * Parámetros: producto y cantidad a ajustar (negativo para descontar).
         -----------------------------------------------------------------------------------------------------*/
        public void AjustarStock(Producto producto, int cantidad)
        {
            try
            {
                /* Aplica el ajuste de stock */
                producto.Stock += cantidad;

                /* Se verifica que el stock no quede negativo */
                if (producto.Stock < 0)
                    throw new Exception("No hay suficiente stock disponible.");

                /* Guarda el nuevo stock en el XML */
                productoDAO.Update(producto);

                /* Se verifica si se llegó al stock mínimo para emitir alerta automática (CU10) */
                if (producto.Stock <= producto.StockMinimo)
                    stockManager.EmitirAlerta(producto.Id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ajustar el stock: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: DesactivarProducto
         * Descripción: Baja lógica del producto. Verifica que exista antes de desactivarlo.
         * Parámetros: ID del producto.
         -----------------------------------------------------------------------------------------------------*/
        public void DesactivarProducto(int id)
        {
            try
            {
                /* Verifica que el producto exista */
                Producto producto = productoDAO.GetById(id);
                if (producto == null)
                    throw new Exception("El producto no existe.");

                /* Verifica que el producto no esté ya desactivado */
                if (!producto.Activo)
                    throw new Exception("El producto ya está desactivado.");

                /* Baja lógica: el producto queda en el XML pero con Activo = false */
                productoDAO.Deactivate(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al desactivar el producto: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: ActivarProducto
         * Descripción: Reactiva un producto previamente desactivado.
         * Parámetros: ID del producto.
         -----------------------------------------------------------------------------------------------------*/
        public void ActivarProducto(int id)
        {
            try
            {
                /* Verifica que el producto exista */
                Producto producto = productoDAO.GetById(id);
                if (producto == null)
                    throw new Exception("El producto no existe.");

                /* Verifica que el producto no esté ya activo */
                if (producto.Activo)
                    throw new Exception("El producto ya está activo.");

                /* Reactiva el producto en el XML */
                producto.Activo = true;
                productoDAO.Update(producto);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al activar el producto: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
        MÉTODO PRIVADO
         -----------------------------------------------------------------------------------------------------*/

        /* -----------------------------------------------------------------------------------------------------
         * Función: ValidarProducto
         * Descripción: Centraliza todas las validaciones de negocio para alta y modificación.
         *              Se llama tanto en RegistrarProducto como en ModificarProducto.
         * Parámetros: producto con datos en texto plano.
         -----------------------------------------------------------------------------------------------------*/
        private void ValidarProducto(Producto producto)
        {
            /* Validaciones de campos obligatorios */
            if (string.IsNullOrEmpty(producto.Modelo))
                throw new Exception("El modelo es obligatorio.");

            if (string.IsNullOrEmpty(producto.Color))
                throw new Exception("El color es obligatorio.");

            if (producto.IdProveedor <= 0)
                throw new Exception("Debe asignar un proveedor al producto.");

            if (producto.Talle <= 0)
                throw new Exception("El talle debe ser mayor a cero.");

            /* Validaciones numéricas */
            if (producto.Precio <= 0)
                throw new Exception("El precio debe ser mayor a cero.");

            if (producto.Stock < 0)
                throw new Exception("El stock no puede ser negativo.");

            if (producto.StockMinimo < 0)
                throw new Exception("El stock mínimo no puede ser negativo.");

            if (producto.StockMinimo > producto.Stock)
                throw new Exception("El stock mínimo no puede superar el stock actual.");
        }
    }
}
