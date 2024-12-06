using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BookSystem.Context;
using BookSystem.DomainModels;
using BookSystem.DomainModels; // Adjust this namespace according to your project structure
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                    var existingGenre = await _context.Genres.FirstOrDefaultAsync(g =>
                        g.Name.ToLower() == genre.Name.ToLower()
                    );

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

        [HttpPost("withimage")]
        public async Task<IActionResult> CreateBook([FromForm] CreateBookDto createBookDto)
        {
            try
            {
                // Validate incoming data
                if (createBookDto == null)
                {
                    return BadRequest("Book data is required.");
                }

                // Create the Book entity
                var book = new Book
                {
                    BookTitle = createBookDto.BookTitle,
                    BookPages = createBookDto.BookPages,
                    BookSummary = createBookDto.BookSummary,
                    BookType = createBookDto.BookType,
                };

                // Process the image and save it as binary data
                if (createBookDto.CoverImage != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await createBookDto.CoverImage.CopyToAsync(memoryStream);
                        book.CoverImage = memoryStream.ToArray();
                    }
                }

                // Process genres if provided
                if (createBookDto == null)
                {
                    return BadRequest("Book data is required.");
                }

               

               


               
                _context.Books.Add(book);
                await _context.SaveChangesAsync();

                if (createBookDto.Genres != null && createBookDto.Genres.Any())
                {
                    foreach (var genreId in createBookDto.Genres)
                    {
                       
                        var genre = await _context.Genres.FindAsync(genreId);
                        if (genre != null)
                        {
                            
                            book.Genres.Add(genre);
                        }
                        else
                        {
                            
                            return BadRequest($"genre with id {genreId} does not exist");
                        }
                    }

                    // Save changes to the database after linking genres
                    await _context.SaveChangesAsync();
                }

                // Return success response
                return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                // Catch any unexpected errors
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpGet("genre/{genre}")]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooksByGenre(string genre)
        {
            var genreLower = genre.ToLower(); // Convert the input genre to lowercase

            var books = await _context
                .Books.Where(b => b.Genres.Any(g => g.Name.ToLower() == genreLower)) // Use lowercase for comparison
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
        public async Task<ActionResult<IEnumerable<GetBookDTO>>> GetAllBooks()
        {
            var books = await _context.Books.Include(b => b.Genres).ToListAsync();
            var bookDTOs = books
                .Select(b => new GetBookDTO
                {
                    BookTitle = b.BookTitle,
                    BookPages = b.BookPages,
                    BookSummary = b.BookSummary,
                    CoverImage = b.CoverImage,
                    BookType = b.BookType,
                    Genres = b.Genres.Select(g => new GenreDTO { Name = g.Name }).ToList()
                })
                .ToList();
            return Ok(bookDTOs);
        }
    }
}
