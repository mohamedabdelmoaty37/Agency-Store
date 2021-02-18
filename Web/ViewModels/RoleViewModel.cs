using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class RoleViewModel
    {
        [Required]
        public string RoleName { get; set; }

        public int RoleId { get; set; }

        public  List<IdentityRole> ListRole { get; set; }

    }
}
