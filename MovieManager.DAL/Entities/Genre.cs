using System;
using System.Collections.Generic;
using System.Text;

namespace MovieManager.DAL.Entities
{
    /// <summary>
    /// Classe Entità del Data Access Layer (DAL).
    /// Mappa esattamente una tabella fisica nel database relazionale gestito da Entity Framework Core.
    /// </summary>
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
