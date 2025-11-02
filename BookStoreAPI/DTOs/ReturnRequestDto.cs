using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.DTOs
{
    public class ReturnRequestDto
    {
        [Required]
        public int MemberId { get; set; }
        [Required]
        public int BookId { get; set; }
    }
}
