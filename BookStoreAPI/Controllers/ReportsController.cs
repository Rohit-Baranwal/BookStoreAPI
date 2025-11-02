using BookStoreAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class ReportsController : Controller
    {
        private readonly BookStoreContext _context;

        public ReportsController(BookStoreContext context)
        {
            _context = context;
        }

        // GET /api/reports/top-borrowed
        [HttpGet("top-borrowed")]
        public async Task<IActionResult> GetTopBorrowed()
        {
            var top5 = await _context.BorrowRecords
                .GroupBy(br => br.BookId)
                .Select(g => new {
                    BookId = g.Key,
                    TotalBorrows = g.Count()
                })
                .OrderByDescending(x => x.TotalBorrows)
                .Take(5)
                .Join(_context.Books, t => t.BookId, b => b.BookId, (t, b) => new {
                    b.BookId,
                    b.Title,
                    b.Author,
                    t.TotalBorrows
                })
                .ToListAsync();

            return Ok(top5);
        }

        // GET /api/reports/overdue
        [HttpGet("overdue")]
        public async Task<IActionResult> GetOverdue()
        {
            var overdue = await _context.BorrowRecords
                .Where(br => !br.IsReturned && br.BorrowDate.AddDays(14) < DateTime.UtcNow.Date) 
                .Include(br => br.Member)
                .Include(br => br.Book)
                .Select(br => new {
                    br.BorrowId,
                    MemberId = br.MemberId,
                    MemberName = br.Member.Name,
                    BookId = br.BookId,
                    BookTitle = br.Book.Title,
                    br.BorrowDate,
                    DueDate = br.BorrowDate.AddDays(14)
                })
                .ToListAsync();

            return Ok(overdue);
        }

    }
}
