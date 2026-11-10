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
     * Clase: ProveedorDAO
     * Descripción: Capa de acceso a datos para la entidad Proveedor.
     *              Utiliza ProveedorORM para realizar las operaciones de persistencia en el archivo XML.
     *              Incluye métodos específicos para obtener proveedores con deuda pendiente (CU06).
     -----------------------------------------------------------------------------------------------------*/
    public class ProveedorDAO
    {
        private ProveedorORM orm = new ProveedorORM();

        public List<Proveedor> GetAllOrFiltered(string nombre = null, bool? activo = null)
        {
            try
            {
                /* Obtiene todos los proveedores del XML sin filtrar */
                List<Proveedor> proveedores = orm.ObtenerTodos();

                /* Si se proporcionó un nombre, se aplica el filtro (sin distinguir mayúsculas) */
                if (!string.IsNullOrEmpty(nombre))
                {
                    proveedores = proveedores.Where(
                        p => p.Nombre.ToUpper()
                        .Contains(nombre.ToUpper())).ToList();
                }

                /* Si se proporcionó un estado, se aplica el filtro */
                if (activo.HasValue)
                {
                    proveedores = proveedores.Where(p => p.Activo == activo.Value).ToList();
                }

                /* Devuelve los proveedores. */
                return proveedores;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los proveedores: " + ex.Message);
            }
        }

        public Proveedor GetById(int id)
        {
            try
            {
                return orm.ObtenerPorId(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el proveedor: " + ex.Message);
            }
        }

        /* Proveedores con deuda pendiente para el módulo de pagos (CU06) */
        public List<Proveedor> GetWithDebt()
        {
            try
            {
                return orm.ObtenerTodos()
                          .Where(p => p.Activo && p.DeudaTotal > 0)
                          .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener proveedores con deuda: " + ex.Message);
            }
        }

        public void Insert(Proveedor proveedor)
        {
            try
            {
                orm.Insertar(proveedor);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el proveedor: " + ex.Message);
            }
        }

        public void Update(Proveedor proveedor)
        {
            try
            {
                orm.Actualizar(proveedor);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el proveedor: " + ex.Message);
            }
        }

        /* Baja lógica: no se elimina, se desactiva */
        public void Deactivate(int id)
        {
            try
            {
                Proveedor proveedor = orm.ObtenerPorId(id);
                proveedor.Activo = false;
                orm.Actualizar(proveedor);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al desactivar el proveedor: " + ex.Message);
            }
        }
    }
}
