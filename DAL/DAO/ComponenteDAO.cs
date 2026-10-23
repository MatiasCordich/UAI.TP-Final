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
     * Clase: ComponenteDAO
     * Descripción: Capa de acceso a datos para la entidad Componente (Leaf y Composite).
     *              Utiliza ComponenteORM para realizar las operaciones de persistencia en el archivo XML.
     *              Los componentes que devuelve ya tienen el árbol armado: cada Composite trae sus Hijos.
     *              Los componentes no tienen baja lógica (no existe un campo Activo): Delete los elimina
     *              del archivo y los quita de los Composite que los contenían.
     -----------------------------------------------------------------------------------------------------*/
    public class ComponenteDAO
    {
        /* Instancia del ORM de componentes para acceder al XML */
        private ComponenteORM orm = new ComponenteORM();

        /* -----------------------------------------------------------------------------------------------------
         * Función: GetAllOrFiltered
         * Descripción: Obtiene TODOS los componentes (Leaf y Composite).
         *              A su vez tiene un parámetro opcional si se quiere filtrar la búsqueda.
         * Parámetros:
         *              nombre → filtra por nombre del componente (contiene). Si es null, devuelve todos.
         -----------------------------------------------------------------------------------------------------*/
        public List<Componente> GetAllOrFiltered(string nombre = null)
        {
            try
            {
                /* Obtiene todos los componentes del XML, con el árbol ya armado */
                List<Componente> componentes = orm.ObtenerTodos();

                /* Si se proporcionó un nombre, se aplica el filtro (sin distinguir mayúsculas) */
                if (!string.IsNullOrEmpty(nombre))
                {
                    componentes = componentes.Where(
                        c => c.Nombre.ToUpper()
                        .Contains(nombre.ToUpper())).ToList();
                }

                /* Devuelve los componentes. */
                return componentes;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los componentes: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: GetById
         * Descripción: Obtiene un componente por su ID. Si es un Composite, trae su árbol de hijos.
         * Parámetros: id → id del componente.
         -----------------------------------------------------------------------------------------------------*/
        public Componente GetById(int id)
        {
            try
            {
                /* Busca el componente por ID. Si no existe, devuelve null. */
                return orm.ObtenerPorId(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el componente: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: Insert
         * Descripción: Registra el componente. El ORM le asigna el Id.
         *              Si es un Composite, sus hijos deben estar guardados antes (necesitan tener Id).
         * Parámetros: El Leaf o Composite a registrar.
         -----------------------------------------------------------------------------------------------------*/
        public void Insert(Componente componente)
        {
            try
            {
                /* Llama a la función Insertar() del ORM de Componente. */
                orm.Insertar(componente);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el componente: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: Update
         * Descripción: Modifica el componente. En un Composite también actualiza su lista de hijos.
         * Parámetros: El Leaf o Composite con los datos nuevos.
         -----------------------------------------------------------------------------------------------------*/
        public void Update(Componente componente)
        {
            try
            {
                /* Llama a la función Actualizar() del ORM de Componente. */
                orm.Actualizar(componente);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el componente: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: Delete
         * Descripción: Elimina el componente y lo quita de todos los Composite que lo contenían.
         *              Este método no controla si un Rol lo usa como raíz: ese control lo hace la BLL.
         * Parámetros: El id del componente a eliminar.
         -----------------------------------------------------------------------------------------------------*/
        public void Delete(int id)
        {
            try
            {
                orm.Eliminar(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el componente: " + ex.Message);
            }
        }
    }
}
