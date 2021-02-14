using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class PortfolioViewModel
    {
        public Guid Id { get; set; }
        [Required]

        [Display(Name = "NameCatogry ")]
        public string NameCatogry { get; set; }

        [Required]

        [Display(Name = "ProjectName")]
        public string Description { get; set; }


        [Display(Name = "ProjectName")]
        public string ImageUrl { get; set; }

        [Required]

        [Display(Name = "Image")]
        public IFormFile File { get; set; }
        [Required]

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]

        [Display(Name = "Gender")]
        public string Gender { get; set; }
        [Required]

        [Display(Name = "Date")]
        public DateTime Date { get; set; }
        [Required]

        [Display(Name = "Company Name")]
        public string Company { get; set; }
        [Required]

        [Display(Name = "location")]
        public string location { get; set; }

        [Required]

        [Display(Name = "typeName")]

        public Guid typeId { get; set; }
        
    }
}
