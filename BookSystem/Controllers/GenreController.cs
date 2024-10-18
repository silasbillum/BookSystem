using BookSystem.Context;
using BookSystem.DomainModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenreController : Controller
    {
        private readonly AppDBContext _context;

        public GenreController(AppDBContext context)
        {
            _context = context;
        }

        // GET: api/genres
        [HttpGet]
        public async Task<IActionResult> GetGenres()
        {
            var genres = await _context.Genres.ToListAsync();
            return Ok(genres);
        }

        // GET: api/genres/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGenre(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
            {
                return NotFound("Genre not found.");
            }

            return Ok(genre);
        }

        // POST: api/genres
        [HttpPost]
        public async Task<IActionResult> CreateGenre([FromBody] GenreDTO genreDTO)
        {
            if (genreDTO == null)
            {
                return BadRequest("Genre data is required.");
            }
            var genre = new Genre
            {
                Name = genreDTO.Name
                // You can also initialize the Books collection if needed, but typically you won't
            };

            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGenre), new { id = genre.Id }, genre);
        }

        // PUT: api/genres/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGenre(int id, [FromBody] Genre updatedGenre)
        {
            if (id != updatedGenre.Id)
            {
                return BadRequest("Genre ID mismatch.");
            }

            _context.Entry(updatedGenre).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenreExists(id))
                {
                    return NotFound("Genre not found.");
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/genres/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
            {
                return NotFound("Genre not found.");
            }

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GenreExists(int id)
        {
            return _context.Genres.Any(e => e.Id == id);
        }
    }
}

