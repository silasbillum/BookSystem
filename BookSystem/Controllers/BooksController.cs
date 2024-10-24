﻿using BookSystem.Context;
using BookSystem.DomainModels;
using BookSystem.DomainModels; // Adjust this namespace according to your project structure
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly AppDBContext _context;

        public BooksController(AppDBContext context)
        {
            _context = context;
        }

        // POST: api/books
        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] Book newBook)
        {
            if (newBook == null)
            {
                return BadRequest("Book data is required.");
            }

            if (newBook.Genres != null && newBook.Genres.Any())
            {
                var genresToAdd = new List<Genre>();

                foreach (var genre in newBook.Genres)
                {
                    // Case-insensitive comparison by converting both to lowercase
                    var existingGenre = await _context.Genres
                        .FirstOrDefaultAsync(g => g.Name.ToLower() == genre.Name.ToLower());

                    if (existingGenre == null)
                    {
                        // If the genre does not exist, create a new one
                        existingGenre = new Genre { Name = genre.Name };
                        _context.Genres.Add(existingGenre);
                    }

                    // Add the genre to the temporary list to avoid modifying the collection during iteration
                    genresToAdd.Add(existingGenre);
                }

                // Now add the collected genres to the newBook's Genres collection after iteration
                newBook.Genres = genresToAdd;
            }

            // Add the new book to the database
            _context.Books.Add(newBook);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBook), new { id = newBook.Id }, newBook);
        }



        // GET: api/books/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound("Book not found.");
            }

            return Ok(book);
        }
        [HttpPost("with image")]
        public async Task<IActionResult> CreateBook([FromForm] CreateBookDto createBookDto)
        {
            if (createBookDto.CoverImage == null || createBookDto.CoverImage.Length == 0)
            {
                return BadRequest("Cover image is required.");
            }

            var book = new Book
            {
                BookTitle = createBookDto.BookTitle,
                BookPages = createBookDto.BookPages,
                BookSummary = createBookDto.BookSummary,
                // Additional properties as needed
            };

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await createBookDto.CoverImage.CopyToAsync(memoryStream);
                    book.CoverImage = memoryStream.ToArray();
                }

                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("genre/{genre}")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooksByGenre(string genre)
        {
            var genreLower = genre.ToLower(); // Convert the input genre to lowercase

            var books = await _context.Books
                .Where(b => b.Genres.Any(g => g.Name.ToLower() == genreLower)) // Use lowercase for comparison
                .ToListAsync();

            if (books == null || !books.Any())
            {
                return NotFound();
            }

            return Ok(books);
        }



        // PUT: api/books/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] Book updatedBook)
        {
            if (updatedBook == null || id != updatedBook.Id)
            {
                return BadRequest("Invalid book data.");
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound("Book not found.");
            }

            // Update the book's properties
            book.BookTitle = updatedBook.BookTitle;
            book.BookPages = updatedBook.BookPages;
            book.BookSummary = updatedBook.BookSummary;
            book.CoverImage = updatedBook.CoverImage; // Update cover image if provided
            book.Genres = updatedBook.Genres; // Update genres

            await _context.SaveChangesAsync();
            return NoContent(); // Indicate that the update was successful
        }

        [HttpPost("{id}/uploadCoverImage")]
        public async Task<IActionResult> UploadCoverImage(int id, IFormFile coverImage)
        {
            if (coverImage == null || coverImage.Length == 0)
            {
                return BadRequest("No file provided.");
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound("Book not found.");
            }

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await coverImage.CopyToAsync(memoryStream);
                    book.CoverImage = memoryStream.ToArray();
                }

                await _context.SaveChangesAsync();
                return Ok("Cover image uploaded successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}/coverImage")]
        public async Task<IActionResult> GetCoverImage(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null || book.CoverImage == null)
            {
                return NotFound("Image not found.");
            }

            var base64Image = Convert.ToBase64String(book.CoverImage);
            var imgSrc = $"data:image/jpeg;base64,{base64Image}"; // Adjust MIME type as needed

            return Ok(new { ImageSrc = imgSrc });
        }

        // GET: api/books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks()
        {
            var books = await _context.Books.ToListAsync();
            return Ok(books);
        }
    }
}

