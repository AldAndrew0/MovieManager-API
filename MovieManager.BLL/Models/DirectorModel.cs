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
    public class DirectorModel : IModelWithId
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateOnly? BirthDate { get; set; }
        public string? Country { get; set; }
        public string? Biography { get; set; }
    }
}
