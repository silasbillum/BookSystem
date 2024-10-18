using BookSystem.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace BookSystem.Context
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .Property(b => b.BookTitle)
                .HasColumnType("character varying(255)");

            modelBuilder.Entity<Book>()
                .Property(b => b.BookPages)
                .HasColumnType("text"); // or character varying if you have a length limit

            modelBuilder.Entity<Book>()
                .Property(b => b.BookSummary)
                .HasColumnType("text");

            modelBuilder.Entity<Book>()
                .Property(b => b.CoverImage)
                .HasColumnType("bytea");

            // Similarly for Genre entity
            modelBuilder.Entity<Genre>()
                .Property(g => g.Name)
                .HasColumnType("character varying(255)");
        }
    }
}

