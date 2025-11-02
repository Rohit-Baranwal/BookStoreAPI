using BookStoreAPI.Data;
using BookStoreAPI.DTOs;
using BookStoreAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MembersController : Controller
    {
        private readonly BookStoreContext _context;

        public MembersController(BookStoreContext context)
        {
            _context = context;
        }

        // POST /api/members
        [HttpPost]
        public async Task<IActionResult> CreateMember([FromBody] MemberCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (await _context.Members.AnyAsync(m => m.Email == dto.Email))
            {
                ModelState.AddModelError(nameof(dto.Email), "Email already exists!!");
                return Conflict(ModelState);
            }

            var member = new Member
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                JoinDate = DateTime.UtcNow.Date
            };

            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateMember), new { id = member.MemberId }, member);
        }
    }
}
