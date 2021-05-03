using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KnifeCompany.Models
{
    public class Category
    {
        public int Id { get; set; }


        [Display(Name="Category Name")]
        [Required]
        [MaxLength(100)]
        public String Name { get; set; }

    }
}
