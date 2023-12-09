using System.ComponentModel.DataAnnotations;

namespace FPT_BOOKMVC.ModelsCRUD.Category
{
    public class AddCategoryViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
