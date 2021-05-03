using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KnifeCompany.Models
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }

        [Required]
        public String Name { get; set; }

        [Required]
        public String Email { get; set; }

        [Required]
        public String PhoneNo { get; set; }

        [Required]
        public String Address { get; set; }

        [Required]
        public String Password { get; set; }


    }
}

