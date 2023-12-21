using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FPT_BOOKMVC.Models
{
	public class Cart
	{
		[Key]
		public int CartId { get; set; }
		[Required]
		public int Quantity { get; set; }
		[Required]
		public int BookId { get; set; }


		[ForeignKey("BookId")] //lk với cart
		public Book Book { get; set; }
		[Required]
		public decimal Total { get; set; }
	}
}
