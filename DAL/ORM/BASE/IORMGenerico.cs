using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ORM.BASE
{
    /* -----------------------------------------------------------------------------------------------------
     * Interfaz: IOrmGenerico<T>
     * Descripción: Define la estructura que debe cumplir cualquier ORM del sistema.
     *              Al ser genérica con <T>, sirve para cualquier entidad sin repetir código.
     *              Establece las operaciones básicas de persistencia: obtener, insertar, 
     *              actualizar y eliminar. Toda clase ORM del sistema debe implementarla.
     -----------------------------------------------------------------------------------------------------*/
    public interface IOrmGenerico<T> where T : class
    {
        int ObtenerSiguienteId();
        List<T> ObtenerTodos();
        T ObtenerPorId(int id);
        void Insertar(T entidad);
        void Actualizar(T entidad);
        void Eliminar(int id);
    }
}
