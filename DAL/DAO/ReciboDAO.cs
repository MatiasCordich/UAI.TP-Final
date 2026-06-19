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
     * Clase: ReciboDAO
     * Descripción: Capa de acceso a datos para la entidad Recibo.
     *              Utiliza ReciboORM para realizar las operaciones de persistencia en el archivo XML.
     *              El recibo se genera automáticamente al confirmar un pago (CU07).
     -----------------------------------------------------------------------------------------------------*/
    public class ReciboDAO
    {
        private ReciboORM orm = new ReciboORM();

        public List<Recibo> GetAll()
        {
            try
            {
                return orm.ObtenerTodos();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los recibos: " + ex.Message);
            }
        }

        public Recibo GetById(int id)
        {
            try
            {
                return orm.ObtenerPorId(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el recibo: " + ex.Message);
            }
        }

        /* Recibo asociado a un pago específico (CU07) */
        public Recibo GetByPayment(int idPago)
        {
            try
            {
                return orm.ObtenerTodos()
                          .FirstOrDefault(r => r.IdPago == idPago);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el recibo del pago: " + ex.Message);
            }
        }

        public void Insert(Recibo recibo)
        {
            try
            {
                orm.Insertar(recibo);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el recibo: " + ex.Message);
            }
        }
    }
}
