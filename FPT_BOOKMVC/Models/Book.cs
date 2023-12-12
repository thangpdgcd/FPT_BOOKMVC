using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FPT_BOOKMVC.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime UpdateDate { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int PublishCompanyId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        [ForeignKey("PublishCompanyId")]
        public PublishCompany PublishCompany { get; set; }

        [Required(ErrorMessage = "Please choose Front image")]
        [Display(Name = "Front Image")]
        [NotMapped]
        public IFormFile FronImage { get; set; }
    }
}
