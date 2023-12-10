using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FPT_BOOKMVC.ModelsCRUD.Book
{
    public class UpdateBookView
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
        public Models.Category Category { get; set; }
        [ForeignKey("PublishCompanyId")]
        public Models.PushlishCompany PublishCompany { get; set; }

        [Required(ErrorMessage = "Please choose Front image")]
        [Display(Name = "Front Image")]
        [NotMapped]
        public IFormFile FronImage { get; set; }
    }
}
