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
     * Clase: RolDAO
     * Descripción: Capa de acceso a datos para la entidad Rol.
     *              Utiliza RolORM para realizar las operaciones de persistencia en el archivo XML.
     *              Expone métodos con nombres en inglés siguiendo la convención del proyecto.
     -----------------------------------------------------------------------------------------------------*/
    public class RolDAO
    {
        private RolORM orm = new RolORM();

        public List<Rol> GetAll()
        {
            try
            {
                return orm.ObtenerTodos();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los roles: " + ex.Message);
            }
        }

        public Rol GetById(int id)
        {
            try
            {
                return orm.ObtenerPorId(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el rol: " + ex.Message);
            }
        }

        public void Insert(Rol rol)
        {
            try
            {
                orm.Insertar(rol);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el rol: " + ex.Message);
            }
        }

        public void Update(Rol rol)
        {
            try
            {
                orm.Actualizar(rol);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el rol: " + ex.Message);
            }
        }

        public void Delete(int id)
        {
            try
            {
                orm.Eliminar(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el rol: " + ex.Message);
            }
        }
    }
}
