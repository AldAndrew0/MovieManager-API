using System;
using System.Collections.Generic;
using System.Text;

namespace MovieManager.BLL.Models
{
    /// <summary>
    /// Data Transfer Object (Model) per l'entità. 
    /// Utilizzato dal Business Logic Layer (BLL) e dall'API per scambiare dati 
    /// senza esporre direttamente le entità del database (DAL).
    /// </summary>
    public class GenreModel : IModelWithId
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
