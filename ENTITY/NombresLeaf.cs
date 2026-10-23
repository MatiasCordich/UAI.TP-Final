using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    /*------------------------------------------------------------------------------------------------------------------
    * ENTIDAD: NombresLeaf
    * Se concentra en un sólo lugar los nombres de todos los Leaf (permisos individuales) del sistema, para que 
    * la UI y el inicializador de datos usen siempre la misma constante y no repitan textos sueltos. 
    * Cada Leaf corresponde a un ítem del menu de FrmPrincipal. 
    * 
    * Para entenderlo fácil: es la lista con los nombres de los 14 "archivos" del árbol de permisos.
    * Cada constante es solo el texto del nombre, no un objeto Leaf.
    ------------------------------------------------------------------------------------------------------------------*/

    /* "static" significa que no se crean objetos de esta clase: se usa directo, ej: NombresLeaf.NuevaVenta.
   "const" significa que el valor es fijo y no se puede cambiar mientras el programa corre.
   Sirve para no escribir "Nueva Venta" a mano en muchos lugares y evitar errores de tipeo. */
    public static class NombresLeaf
    {
        /* Ventas */
        public const string NuevaVenta = "Nueva Venta"; 
        public const string PostVenta = "Post Venta";
        public const string GestionarClientes = "Gestionar Clientes";

        /* Compras */
        public const string Pedidos = "Pedidos";
        public const string Recepcion = "Recepcion";

        /* Pagos */
        public const string RegistrarPago = "Registrar Pago";
        public const string Conciliacion = "Conciliacion";

        /* Stock */
        public const string AjusteStock = "Ajuste Stock";
        public const string Alertas = "Alertas";

        /* Fidelización */
        public const string Cupones = "Cupones";

        /* Reportes */
        public const string Dashboard = "Dashboard";

        /* Administración */
        public const string GestionarUsuarios = "Gestionar Usuarios";
        public const string GestionarProductos = "Gestionar Productos";
        public const string GestionarProveedores = "Gestionar Proveedores";
    }
}
