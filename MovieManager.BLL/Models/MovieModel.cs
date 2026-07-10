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
    public class MovieModel : IModelWithId
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? OriginalTitle { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public int DurationMinutes { get; set; }
        public string? Synopsis { get; set; }
        public string Language { get; set; } = string.Empty;
        public string? Country { get; set; }
        public decimal? Budget { get; set; }
        public decimal? Revenue { get; set; }
        public string? PosterUrl { get; set; }
        public string? AgeRating { get; set; }
        public int GenreId { get; set; }
        public int DirectorId { get; set; }
    }
}
