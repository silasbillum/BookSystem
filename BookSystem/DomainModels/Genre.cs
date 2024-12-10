using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookSystem.DomainModels
{
    public class Genre
    {
        public int Id { get; set; } // Primary Key
        public string Name { get; set; } // Maps to 'varchar' in PostgreSQL

        [JsonIgnore]
        public ICollection<Book> Books { get; set; } = new List<Book>(); // Initialize the collection
    }

    public class GenreDTO
    {

        public int Id { get; set; } // Primary Key
        public string Name { get; set; }
    }
}
