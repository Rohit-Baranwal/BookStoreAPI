using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.Models
{
    public class Member
    {
        public int MemberId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(15)]
        public string Phone {  get; set; }

        public DateTime JoinDate { get; set; } =  DateTime.Now.Date;
    }
}
