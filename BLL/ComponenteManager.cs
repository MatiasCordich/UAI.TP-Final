using DAL.DAO;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL
{
    /* -----------------------------------------------------------------------------------------------------
     * Clase: ComponenteManager
     * Descripción: Capa de lógica de negocio para el árbol de permisos (patrón Composite).
     *              Se encarga de:
     *              - Crear el árbol inicial de permisos de los 5 roles del sistema.
     *              - Asignar a cada rol su Composite raíz.
     *              - Listar componentes y eliminarlos con sus validaciones.
     *
     *              Estructura del árbol que crea:
     *              Rol (raíz) → Módulos (Composite) → Leafs (permisos sueltos)
     -----------------------------------------------------------------------------------------------------*/
    public class ComponenteManager
    {
        /* Instancia del DAO de componentes para acceder al XML */
        private ComponenteDAO componenteDAO = new ComponenteDAO();

        /* Instancia del DAO de roles para asignarle a cada rol su Composite raíz */
        private RolDAO rolDAO = new RolDAO();

        /* -----------------------------------------------------------------------------------------------------
         * Función: ListarComponentesOFiltrados
         * Descripción: Obtiene todos los componentes (Leaf y Composite).
         *              A su vez tiene un parámetro opcional si se quiere filtrar la búsqueda.
         * Parámetros: nombre → filtra por nombre del componente (contiene). Si es null, devuelve todos.
         * Retorna: lista de componentes.
         -----------------------------------------------------------------------------------------------------*/
        public List<Componente> ListarComponentesOFiltrados(string nombre = null)
        {
            try
            {
                /* Delega el filtrado al DAO con el parámetro opcional */
                return componenteDAO.GetAllOrFiltered(nombre);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar los componentes: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: EliminarComponente
         * Descripción: Elimina un componente validando que ningún rol lo use como raíz.
         *              Si se elimina, el DAO también lo quita de los Composite que lo contenían.
         * Parámetros: id → id del componente a eliminar.
         -----------------------------------------------------------------------------------------------------*/
        public void EliminarComponente(int id)
        {
            try
            {
                /* Verifica que el componente exista */
                Componente componente = componenteDAO.GetById(id);
                if (componente == null)
                    throw new Exception("El componente no existe.");

                /* Verifica que ningún rol lo tenga asignado como su árbol de permisos.
                   Si se eliminara, ese rol quedaría sin permisos y los usuarios no podrían usar el sistema */
                if (rolDAO.GetAll().Exists(r => r.IdComponente == id))
                    throw new Exception("No se puede eliminar: es el árbol de permisos de un rol.");

                /* Elimina el componente del XML */
                componenteDAO.Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el componente: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: InicializarComponentes
         * Descripción: Deja el sistema listo para usar permisos. Se puede ejecutar todas las veces
         *              que haga falta porque no duplica nada:
         *              1) Si no existe ningún componente, crea el árbol completo.
         *              2) Asigna la raíz a los roles que todavía no tienen permisos.
         *              Debe ejecutarse después de que los roles ya estén creados.
         -----------------------------------------------------------------------------------------------------*/
        public void InicializarComponentes()
        {
            try
            {
                /* Si todavía no hay componentes guardados, crea el árbol inicial completo */
                if (componenteDAO.GetAllOrFiltered().Count == 0)
                    CrearArbolInicial();

                /* Conecta cada rol con su Composite raíz */
                AsignarRaicesARoles();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al inicializar los componentes: " + ex.Message);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: CrearArbolInicial
         * Descripción: Crea y guarda todos los componentes del sistema.
         *              Se crean de abajo hacia arriba (primero los hijos y después los grupos que los
         *              contienen), porque un Composite necesita que sus hijos ya tengan Id para guardarse.
         -----------------------------------------------------------------------------------------------------*/
        private void CrearArbolInicial()
        {
            /* 1. LEAFS: los 14 permisos sueltos, uno por cada ítem del menú */
            Leaf nuevaVenta = CrearLeaf(NombresLeaf.NuevaVenta);
            Leaf postVenta = CrearLeaf(NombresLeaf.PostVenta);
            Leaf gestionarClientes = CrearLeaf(NombresLeaf.GestionarClientes);
            Leaf pedidos = CrearLeaf(NombresLeaf.Pedidos);
            Leaf recepcion = CrearLeaf(NombresLeaf.Recepcion);
            Leaf registrarPago = CrearLeaf(NombresLeaf.RegistrarPago);
            Leaf conciliacion = CrearLeaf(NombresLeaf.Conciliacion);
            Leaf ajusteStock = CrearLeaf(NombresLeaf.AjusteStock);
            Leaf alertas = CrearLeaf(NombresLeaf.Alertas);
            Leaf cupones = CrearLeaf(NombresLeaf.Cupones);
            Leaf dashboard = CrearLeaf(NombresLeaf.Dashboard);
            Leaf gestionarUsuarios = CrearLeaf(NombresLeaf.GestionarUsuarios);
            Leaf gestionarProductos = CrearLeaf(NombresLeaf.GestionarProductos);
            Leaf gestionarProveedores = CrearLeaf(NombresLeaf.GestionarProveedores);

            /* 2. COMPOSITES DE MÓDULO: agrupan los Leafs por área del sistema.
               "Operar ventas" es un grupo dentro de otro grupo: queda adentro de "Módulo Ventas" */
            Composite operarVentas = CrearComposite("Operar ventas", nuevaVenta, postVenta);
            Composite moduloVentas = CrearComposite("Módulo Ventas", operarVentas, gestionarClientes);
            Composite moduloCompras = CrearComposite("Módulo Compras", pedidos, recepcion);
            Composite moduloPagos = CrearComposite("Módulo Pagos", registrarPago, conciliacion);
            Composite moduloStock = CrearComposite("Módulo Stock", ajusteStock, alertas);
            Composite moduloFidelizacion = CrearComposite("Módulo Fidelización", cupones);
            Composite moduloReportes = CrearComposite("Módulo Reportes", dashboard);
            Composite moduloAdministracion = CrearComposite("Módulo Administración",
                                                 gestionarUsuarios, gestionarProductos, gestionarProveedores);

            /* 3. COMPOSITES RAÍZ: uno por rol, con el mismo nombre del rol.
               Un mismo módulo se reutiliza en varios roles sin copiarlo */
            CrearComposite("Dueño", moduloVentas, moduloCompras, moduloPagos, moduloStock,
                                    moduloFidelizacion, moduloReportes, moduloAdministracion);
            CrearComposite("EncargadoVentas", moduloVentas, moduloFidelizacion, moduloReportes);
            CrearComposite("EmpleadoVentas", moduloVentas);
            CrearComposite("EncargadoCompras", moduloCompras, moduloPagos, moduloStock);
            CrearComposite("EncargadoInventario", moduloStock);
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: AsignarRaicesARoles
         * Descripción: A cada rol que todavía no tiene permisos le asigna el Composite raíz que se llama
         *              igual que el rol. Funciona también con roles creados antes de existir este patrón.
         -----------------------------------------------------------------------------------------------------*/
        private void AsignarRaicesARoles()
        {
            /* Lee todos los componentes una sola vez */
            List<Componente> componentes = componenteDAO.GetAllOrFiltered();

            foreach (Rol rol in rolDAO.GetAll())
            {
                /* Si el rol ya tiene permisos asignados, no se toca */
                if (rol.IdComponente != 0)
                    continue;

                /* Busca entre los componentes el Composite que se llama igual que el rol */
                Componente raiz = componentes.Find(c => c is Composite && c.Nombre == rol.Nombre);

                /* Si no hay una raíz para ese rol (por ejemplo un rol creado después), se deja sin asignar */
                if (raiz == null)
                    continue;

                /* Guarda en el rol el Id de su Composite raíz */
                rol.IdComponente = raiz.Id;
                rolDAO.Update(rol);
            }
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: CrearLeaf
         * Descripción: Crea un Leaf con el nombre indicado, lo guarda y lo devuelve ya con su Id.
         -----------------------------------------------------------------------------------------------------*/
        private Leaf CrearLeaf(string nombre)
        {
            Leaf leaf = new Leaf { Nombre = nombre };

            /* Al guardarlo, el ORM le asigna el Id */
            componenteDAO.Insert(leaf);

            return leaf;
        }

        /* -----------------------------------------------------------------------------------------------------
         * Función: CrearComposite
         * Descripción: Crea un Composite con el nombre indicado, le agrega los hijos recibidos,
         *              lo guarda y lo devuelve ya con su Id.
         *              Los hijos deben estar guardados antes, así tienen Id para poder referenciarlos.
         -----------------------------------------------------------------------------------------------------*/
        private Composite CrearComposite(string nombre, params Componente[] hijos)
        {
            Composite compuesto = new Composite { Nombre = nombre };

            /* Agrega cada hijo al grupo. Insert() controla las referencias circulares y duplicados */
            foreach (Componente hijo in hijos)
                compuesto.Insert(hijo);

            /* Al guardarlo, el ORM le asigna el Id y guarda los Id de sus hijos */
            componenteDAO.Insert(compuesto);

            return compuesto;
        }
    }
}
