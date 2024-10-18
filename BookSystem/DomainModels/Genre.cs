﻿using System.ComponentModel.DataAnnotations;

namespace BookSystem.DomainModels
{
    public class Genre
    {
        [Key]
        public int Id { get; set; } // Primary Key
        public string Name { get; set; } // Maps to 'varchar' in PostgreSQL
        public ICollection<Book> Books { get; set; } = new List<Book>(); // Initialize the collection
    }
    public class GenreDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
