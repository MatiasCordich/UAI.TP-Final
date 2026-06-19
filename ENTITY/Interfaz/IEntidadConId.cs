using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITY
{
    /* -----------------------------------------------------------------------------------------------------
     * Interfaz: IEntidadConId
     * Descripción: Sirve para que el OrmXmlBase<T> pueda trabajar con cualquier entidad de forma genérica 
     * sin saber de qué tipo específico se trata.
     -----------------------------------------------------------------------------------------------------*/
    public interface IEntidadConId
    {
        int Id { get; set; }
    }
}
