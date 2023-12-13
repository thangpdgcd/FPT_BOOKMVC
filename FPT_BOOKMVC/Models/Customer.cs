using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FPT_BOOKMVC.Models
{
    public class Customer
    {
        [Key]
        public string UserId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}
