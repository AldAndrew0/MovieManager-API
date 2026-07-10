
/// <summary>
/// Entry-point dell'applicazione Web API.
/// Si occupa di registrare i servizi nel container di Dependency Injection (IoC), 
/// configurare il DbContext, impostare Swagger (OpenAPI) e gestire la pipeline delle richieste HTTP.
/// </summary>

using Microsoft.EntityFrameworkCore;
using MovieManager.BLL.Models;
using MovieManager.BLL.Services;
using MovieManager.BLL.Services.Interfaces;
using MovieManager.DAL.Data;
using MovieManager.DAL.Entities;
using MovieManager.DAL.Repositories;
using MovieManager.DAL.Repositories.Interfaces;
using MovieManager.PL.API.Configurations;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MovieDbString")
    ?? throw new InvalidOperationException("Connection string 'MovieDbString' non trovata ");

builder.Services.AddDbContext<MovieDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IMovieActorRepository, MovieActorRepository>();

builder.Services.AddScoped<IGenericService<ActorModel>, GenericService<Actor, ActorModel>>();
builder.Services.AddScoped<IGenericService<DirectorModel>, GenericService<Director, DirectorModel>>();
builder.Services.AddScoped<IGenericService<GenreModel>, GenericService<Genre, GenreModel>>();
builder.Services.AddScoped<IGenericService<MovieModel>, GenericService<Movie, MovieModel>>();
builder.Services.AddScoped<IGenericService<ReviewModel>, GenericService<Review, ReviewModel>>();
builder.Services.AddScoped<IMovieActorService, MovieActorService>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();