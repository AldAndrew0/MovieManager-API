using System;
using System.Collections.Generic;
using System.Text;

namespace MovieManager.DAL.Entities
{
    /// <summary>
    /// Classe Entità del Data Access Layer (DAL).
    /// Mappa esattamente una tabella fisica nel database relazionale gestito da Entity Framework Core.
    /// </summary>
    public class MovieActor
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; } = null!;
        public int ActorId { get; set; }
        public Actor Actor { get; set; } = null!;
        public string? CharacterName { get; set; }
        public bool isLeadRole { get; set; }
        public int DisplayOrder { get; set; }
    }
}
