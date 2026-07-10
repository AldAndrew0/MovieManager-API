using System;
using System.Collections.Generic;
using System.Text;

namespace MovieManager.DAL.Entities
{
    /// <summary>
    /// Classe Entità del Data Access Layer (DAL).
    /// Mappa esattamente una tabella fisica nel database relazionale gestito da Entity Framework Core.
    /// </summary>
    public class Actor
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateOnly? BirthDate { get; set; }
        public string? Country { get; set; }
        public string? Biography { get; set; }
        public ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();  
    }
}
