using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class UserviewModel
    {

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]


        public string Email { get; set; }
        public string Id { get; set; }

        [Required]

        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required]

        [Display(Name = "LastName")]
        public string LastName { get; set; }



        [Required]

        [Display(Name = "Address")]
        public string Address { get; set; }


        [Display(Name = "ImageUrl")]
        public string ImageUrl { get; set; }
        public IFormFile File { get; set; }



        [Required]

        [Display(Name = "phone ")]
        public string phone { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

    }



}
