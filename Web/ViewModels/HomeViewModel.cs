using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class HomeViewModel
    {
       
        public List<PortfolioItem> PortfolioItems { get; set; }

        public List<TeamMember> TeamMembers { get; set; }

        public List<Typecat> Catogry { get; set; }
    }
}
