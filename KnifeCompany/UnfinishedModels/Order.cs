using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KnifeCompany.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Display(Name = "Order Total Price")]
        [Required]
        public Double TotalPrice { get; set; }

        public String Status { get; set; }

        public DateTime Date { get; set;}

        [ForeignKey("Customer")]
        public int CustomerRefId { get; set; }
        public Customer Customer { get; set; }


        [ForeignKey("Product")]
        public int ProductRefId { get; set; }
        public Product Product { get; set; } 


    }
}
