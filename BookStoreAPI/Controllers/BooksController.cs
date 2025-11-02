using BookStoreAPI.Data;
using BookStoreAPI.DTOs;
using BookStoreAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private readonly BookStoreContext _context;
        private readonly ILogger<BooksController> _logger;

        public BooksController(BookStoreContext context, ILogger<BooksController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET /api/books
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = await _context.Books
                .Select(b => new {
                    b.BookId,
                    b.Title,
                    b.Author,
                    b.ISBN,
                    b.PublishedYear,
                    b.AvailableCopies,
                    IsAvailable = b.AvailableCopies > 0
                })
                .ToListAsync();

            return Ok(books);
        }

        // POST /api/books
        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] BookCreateDto dto)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if(!string.IsNullOrEmpty(dto.ISBN) && await _context.Books.AnyAsync(b => b.ISBN == dto.ISBN))
            {
                ModelState.AddModelError(nameof(dto.ISBN), "ISBN Already Exist!!");
                return BadRequest(ModelState);
            }

            var book = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                ISBN = dto.ISBN,
                PublishedYear = dto.PublishedYear,
                AvailableCopies = dto.AvailableCopies
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new {id = book.BookId}, book);
        }

    }
}
