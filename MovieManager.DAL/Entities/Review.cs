using System;
using System.Collections.Generic;
using System.Text;

namespace MovieManager.DAL.Entities
{
    /// <summary>
    /// Classe Entità del Data Access Layer (DAL).
    /// Mappa esattamente una tabella fisica nel database relazionale gestito da Entity Framework Core.
    /// </summary>
    public class Review
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; } = null!;
        public string ReviewerName { get; set; } = string.Empty;
        public int Score { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
