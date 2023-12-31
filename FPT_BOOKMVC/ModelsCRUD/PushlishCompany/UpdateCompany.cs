﻿using System.ComponentModel.DataAnnotations;

namespace FPT_BOOKMVC.ModelsCRUD.PushlishCompany
{
    public class UpdateCompany
    {
        [Key]
        public int PublishingCompanyId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Adress { get; set; }
    }
}
