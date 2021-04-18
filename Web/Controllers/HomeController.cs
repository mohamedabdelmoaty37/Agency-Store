using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Web.ViewModels;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly IUnitOfWork<PortfolioItem> _portfolio;
        private readonly IUnitOfWork<TeamMember> _TeamMember;
        private readonly IUnitOfWork<Typecat> _Catogry;

        public HomeController(
          
            IUnitOfWork<PortfolioItem> portfolio, IUnitOfWork<Typecat> Catogry, IUnitOfWork<TeamMember> TeamMember)
        {
         
            _portfolio = portfolio;
            _TeamMember = TeamMember;
            _Catogry = Catogry;
        }
        public IActionResult Index()
        {
            var homeViewModel = new HomeViewModel
            {
              
                PortfolioItems = _portfolio.Entity.GetAll().ToList(),
                TeamMembers = _TeamMember.Entity.GetAll().ToList(),
                Catogry=_Catogry.Entity.GetAll().ToList()

            };

            return View(homeViewModel);
        }

        public IActionResult About()
        {
            return View();
        }




    }
}