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
     * Clase: ClienteDAO
     * Descripción: Capa de acceso a datos para la entidad Cliente.
     *              Utiliza ClienteORM para realizar las operaciones de persistencia en el archivo XML.
     *              Incluye métodos específicos para buscar por DNI y obtener clientes
     *              que cumplen años hoy para el envío de cupones (CU11).
     -----------------------------------------------------------------------------------------------------*/
    public class ClienteDAO
    {
        private ClienteORM orm = new ClienteORM();

        public List<Cliente> GetAll()
        {
            try
            {
                return orm.ObtenerTodos().Where(c => c.Activo).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los clientes: " + ex.Message);
            }
        }

        public Cliente GetById(int id)
        {
            try
            {
                return orm.ObtenerPorId(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el cliente: " + ex.Message);
            }
        }

        /* Busca un cliente por su DNI (CU01) */
        public Cliente GetByDni(string dni)
        {
            try
            {
                return orm.ObtenerTodos()
                          .FirstOrDefault(c => c.Dni == dni && c.Activo);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el cliente por DNI: " + ex.Message);
            }
        }

        /* Obtiene clientes que cumplen años hoy para cupones automáticos (CU11) */
        public List<Cliente> GetBirthdayToday()
        {
            try
            {
                DateTime hoy = DateTime.Today;
                return orm.ObtenerTodos()
                          .Where(c => c.Activo
                                   && c.FechaNacimiento.HasValue
                                   && c.FechaNacimiento.Value.Day == hoy.Day
                                   && c.FechaNacimiento.Value.Month == hoy.Month
                                   && !string.IsNullOrEmpty(c.Email))
                          .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los clientes de cumpleaños: " + ex.Message);
            }
        }

        public void Insert(Cliente cliente)
        {
            try
            {
                orm.Insertar(cliente);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el cliente: " + ex.Message);
            }
        }

        public void Update(Cliente cliente)
        {
            try
            {
                orm.Actualizar(cliente);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el cliente: " + ex.Message);
            }
        }

        /* Baja lógica: no se elimina, se desactiva */
        public void Deactivate(int id)
        {
            try
            {
                Cliente cliente = orm.ObtenerPorId(id);
                cliente.Activo = false;
                orm.Actualizar(cliente);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al desactivar el cliente: " + ex.Message);
            }
        }
    }
}
