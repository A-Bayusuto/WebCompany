using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KnifeCompany.Models
{
    public class Product
    {

        [Key]
        public int Id { get; set; }


        [Display(Name = "Product Name")]
        [Required]
        [MaxLength(200)]
        public String Name { get; set; }

        [Required]
        public Double Price { get; set; }

        [Required]
        public Boolean Status { get; set; }
        public String Description { get; set; }
        public String Picture { get; set; }


        //public ICollection<Order> Orders { get; set; }

    }
}