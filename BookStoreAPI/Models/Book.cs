using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.Models
{
    [Index(nameof(ISBN), IsUnique = true)]
    public class Book
    {
        public int BookId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(100)]
        public string Author {  get; set; }

        [MaxLength(50)]
        public string ISBN { get; set; }

        [Range(0, 9999)]
        public int PublishedYear { get; set; }

        [Range(0, int.MaxValue)]
        public int AvailableCopies { get; set; }
    }
}
