using BookStoreAPI.Data;
using BookStoreAPI.DTOs;
using BookStoreAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class BorrowController : Controller
    {
        private readonly BookStoreContext _context;

        public BorrowController(BookStoreContext context)
        {
            _context = context;
        }

        // POST /api/borrow
        [HttpPost("borrow")]
        public async Task<IActionResult> Borrow([FromBody]BorrowRequestDto dto)
        {
            if (!ModelState.IsValid)
            { 
                return BadRequest(ModelState);
            }

            var member = await _context.Members.FindAsync(dto.MemberId);
            if (member == null) return NotFound($"Member {dto.MemberId} Not Found!!");

            var book = await _context.Books.FindAsync(dto.BookId);
            if (book == null) return NotFound($"Book {dto.BookId} Not Found!!");

            if (book.AvailableCopies <= 0) {
                return BadRequest("No Copies Available to borrow!!");
            }

            var borrow = new BorrowRecord
            {
                BookId = dto.BookId,
                MemberId = dto.MemberId,
                BorrowDate = DateTime.Now.Date,
                IsReturned = false
            };

            using var trx = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.BorrowRecords.Add(borrow);
                book.AvailableCopies -= 1;
                await _context.SaveChangesAsync();
                await trx.CommitAsync();
            }
            catch (Exception)
            {
                await trx.RollbackAsync();
                throw;
            }

            return Ok(borrow);
        }

        // POST /api/return
        [HttpPost("return")]
        public async Task<IActionResult> ReturnBook([FromBody] ReturnRequestDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //var borrow = await _context.BorrowRecords
            //    .Include(br => br.Book)
            //    .FirstOrDefaultAsync(br => br.BorrowId == dto.BorrowId);

            //if (borrow == null) return NotFound("Borrow record not found!!");
            //if (borrow.IsReturned) return BadRequest("Book already returned!!");

            var borrow = await _context.BorrowRecords
                            .Include(br => br.Book)
                            .FirstOrDefaultAsync(br =>
                                br.MemberId == dto.MemberId &&
                                br.BookId == dto.BookId &&
                                br.IsReturned == false);

            if (borrow == null)
                return NotFound("Borrow record not found or already returned!");


            borrow.IsReturned = true;
            borrow.ReturnDate = DateTime.UtcNow.Date;
            borrow.Book.AvailableCopies += 1;

            await _context.SaveChangesAsync();

            return Ok(borrow);
        }
    }
}
