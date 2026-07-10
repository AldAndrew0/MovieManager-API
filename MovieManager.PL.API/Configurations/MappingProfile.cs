using AutoMapper;
using MovieManager.BLL.Models;
using MovieManager.DAL.Entities;

namespace MovieManager.PL.API.Configurations
{
    /// <summary>
    /// Configurazione centralizzata di AutoMapper.
    /// Definisce le regole di mappatura bidirezionale tra le Entità del Data Access Layer (DAL)
    /// e i Modelli del Business Logic Layer (BLL), automatizzando la conversione dei dati.
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Actor, ActorModel>().ReverseMap();
            CreateMap<Director, DirectorModel>().ReverseMap();
            CreateMap<Genre, GenreModel>().ReverseMap();
            CreateMap<Movie, MovieModel>().ReverseMap();
            CreateMap<Review, ReviewModel>().ReverseMap();
            CreateMap<MovieActor, MovieActorModel>().ReverseMap();
        }
    }
}