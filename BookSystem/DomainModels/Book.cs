using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;  // For DbContext and DbSet

namespace BookSystem.DomainModels

{
    public class Book
    {
        [Key]
        public int Id { get; set; }  // Primary Key
        public string BookTitle { get; set; } // Maps to 'varchar' in PostgreSQL
        public string BookPages { get; set; } // Maps to 'varchar' in PostgreSQL
        
        public string BookSummary { get; set; } // Maps to 'text' in PostgreSQL
        
        public ICollection<Genre> Genres { get; set; } = new List<Genre>(); // Initialize the collection
    }

    public class CreateBookDto
    {
        public string BookTitle { get; set; }
        public string BookPages { get; set; }
        
        public string BookSummary { get; set; }
        public IFormFile CoverImage { get; set; } // For the cover image file
        // Additional properties as needed
        public ICollection<Genre> Genres { get; set; } = new List<Genre>(); // Initialize the collection
    }

}
