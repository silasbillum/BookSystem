using System.ComponentModel.DataAnnotations; // For DbContext and DbSet
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace BookSystem.DomainModels
{
    public class Book
    {
        [Key]
        public int Id { get; set; } // Primary Key
        public string BookTitle { get; set; } // Maps to 'varchar' in PostgreSQL
        public string BookPages { get; set; } // Maps to 'varchar' in PostgreSQL
        public string BookSummary { get; set; } // Maps to 'text' in PostgreSQL
        [JsonIgnore]
        public byte[]? CoverImage { get; set; } // Maps to 'bytea' in PostgreSQL
        public string BookType { get; set; }
        public ICollection<Genre> Genres { get; set; } = new List<Genre>(); // Initialize the collection
    }

    public class CreateBookDto
    {
        
        public string BookTitle { get; set; }
        public string BookPages { get; set; }
        public string BookSummary { get; set; }
        public string BookType { get; set; }
        public ICollection<int> Genres { get; set; } = new List<int>();
        public IFormFile CoverImage { get; set; } // For the cover image file
        // Additional properties as needed
    }
    public class GetBookDTO
    {
        public string BookTitle { get; set; }
        public string BookPages { get; set; }
        public string BookSummary { get; set; }
        public byte[]? CoverImage { get; set; }
        public string BookType { get; set; }
        public ICollection<GenreDTO> Genres { get; set; } = new List<GenreDTO>();
    }
    
}
