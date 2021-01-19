using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Entities
{
   public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Address { get; set; }
        public string Image { get; set; }
        [Required]
        public string LastName { get; set; }





    }
}
