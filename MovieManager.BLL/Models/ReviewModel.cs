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
    public class ReviewModel : IModelWithId
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string ReviewerName { get; set; } = string.Empty;
        public int Score { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
