using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FPT_BOOKMVC.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; }
        [Required]
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }
        [Required]
        public decimal Total { get; set; }
    }
}
