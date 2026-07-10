using System;
using System.Collections.Generic;
using System.Text;

namespace MovieManager.BLL.Models
{
    /// <summary>
    /// Model BLL per la tabella di associazione molti-a-molti tra Movie e Actor.
    /// Non implementa IModelWithId in quanto utilizza una chiave primaria composta (MovieId, ActorId).
    /// </summary>
    public class MovieActorModel
    {
        public int MovieId { get; set; }
        public int ActorId { get; set; }
        public string? CharacterName { get; set; }
        public bool IsLeadRole { get; set; }
        public int DisplayOrder { get; set; }
    }
}
