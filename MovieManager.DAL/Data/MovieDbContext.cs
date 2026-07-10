using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MovieManager.DAL.Entities;

namespace MovieManager.DAL.Data
{
    /// <summary>
    /// Rappresenta la sessione col database (Data Access Layer). 
    /// Contiene i DbSet per ogni entità e configura le relazioni (Fluent API), 
    /// le chiavi composte e i vincoli strutturali tramite il metodo OnModelCreating.
    /// </summary>
    public class MovieDbContext : DbContext
    {
        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<MovieActor> MovieActors { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MovieActor>().HasKey(ma => new { ma.MovieId, ma.ActorId }); 

            modelBuilder.Entity<MovieActor>()
                .HasOne(ma => ma.Movie)
                .WithMany(m => m.MovieActors)
                .HasForeignKey(ma => ma.MovieId);

            modelBuilder.Entity<MovieActor>()
                .HasOne(ma => ma.Actor)
                .WithMany(a => a.MovieActors)
                .HasForeignKey(ma => ma.ActorId);

            modelBuilder.Entity<Movie>()
                .Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<Genre>()
                .Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<Director>(entity =>
            {
                entity.Property(d => d.FirstName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(d => d.LastName)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Actor>(entity =>
            {
                entity.Property(a => a.FirstName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(a => a.LastName)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Review>()
                .Property(r => r.ReviewerName)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<Review>()
              .Property(m => m.Score)
              .IsRequired();

            modelBuilder.Entity<Review>()
            .ToTable(t => t.HasCheckConstraint("CK_Review_Score", "Score >= 1 AND Score <= 10"));

            modelBuilder.Entity<Movie>()
                .Property(m => m.Budget)
                .HasPrecision(18, 2); 

            modelBuilder.Entity<Movie>()
                .Property(m => m.Revenue)
                .HasPrecision(18, 2);
        }
    }
}
