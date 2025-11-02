using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.Models
{
    public class BorrowRecord
    {
        [Key]
        public int BorrowId { get; set; }

        [Required]
        public int MemberId { get; set; }

        public Member Member { get; set; }

        [Required]
        public int BookId { get; set; }

        public Book Book { get; set; }

        public DateTime BorrowDate { get; set; } = DateTime.UtcNow.Date;

        public DateTime? ReturnDate { get; set; }

        public bool IsReturned { get; set; } = false;
    }
}
