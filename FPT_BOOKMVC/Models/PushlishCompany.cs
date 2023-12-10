using System.ComponentModel.DataAnnotations;

namespace FPT_BOOKMVC.Models
{
	public class PushlishCompany
	{
		[Key]
		public int PublishingCompanyId { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string Adress { get; set; }
	}
}
