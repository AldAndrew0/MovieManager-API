using System;
using System.Collections.Generic;
using System.Text;

namespace MovieManager.BLL.Models
{
    /// <summary>
    /// Contratto base per i modelli del BLL che possiedono una chiave primaria singola (Id).
    /// Garantisce ai servizi generici (GenericService) la presenza di un identificatore univoco.
    /// </summary>
    public interface IModelWithId
    {
        int Id { get; set; }
    }
}
