using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class TeamMemberViewModel
    {


        public Guid Id { get; set; }

        [Required]

        [Display(Name = "FullName")]
        public string FullName { get; set; }


        [Required]

        [Display(Name = "TitleJob")]

        public string TitleJob { get; set; }
      

        public string FBUrllink { get; set; }
        public string TWUrllink { get; set; }
        public string LinkedInUrl { get; set; }


  
     
        
        [Display(Name = "ImageUrl")]
        public string ImageUrl { get; set; }







    }

    public class TeamMembercreateViewModel : TeamMemberViewModel
    {

        [Required]
        [Display(Name = "Image")]
        public IFormFile File { get; set; }
    }

    public class TeamMemberEditViewModel : TeamMemberViewModel
    {

      
        [Display(Name = "Image Url")]
        public IFormFile File { get; set; }
    }
}
