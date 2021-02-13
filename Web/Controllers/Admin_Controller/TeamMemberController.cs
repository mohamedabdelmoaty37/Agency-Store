using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.ViewModels;

namespace Web.Controllers.Admin_Controller
{
    public class TeamMemberController : Controller
    {
        private readonly IUnitOfWork<TeamMember> _Team;
        private readonly IHostingEnvironment _hosting;

        public TeamMemberController(IUnitOfWork<TeamMember> Team, IHostingEnvironment hosting)
        {
            _Team = Team;
            _hosting = hosting;
        }

      

        // GET: TeamMemberController
        public ActionResult Index()
        {
            return View(_Team.Entity.GetAll());
        }

        // GET: TeamMemberController/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Member = _Team.Entity.GetById(id);
            if (Member == null)
            {
                return NotFound();
            }

            return View(Member);
        }




        // GET: TeamMemberController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TeamMemberController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TeamMembercreateViewModel Model)
        {

            if (ModelState.IsValid)
            {

                if (Model.File != null)
                {
                    string uploads = Path.Combine(_hosting.WebRootPath, @"AdminRoot\img\Teammember");
                    string fullPath = Path.Combine(uploads, Model.FullName+Model.File.FileName);
                    Model.File.CopyTo(new FileStream(fullPath, FileMode.Create));
                }


                var TeamMember  = new TeamMember
                {
                  FullName = Model.FullName,
                    TitleJob = Model.TitleJob,
                    FBUrllink = Model.FBUrllink,
                    TWUrllink = Model.TWUrllink,
                   LinkedInUrl = Model.LinkedInUrl,
                    
                    ImageUrl = Model.FullName+Model.File.FileName

                };



                  _Team.Entity.Insert(TeamMember);
                  _Team.Save();


                
                return RedirectToAction("Index");

            }


            return View(Model);
        }

        // GET: TeamMemberController/Edit/5
        public ActionResult Edit(Guid id)

        {
            if (id == null)
            {
                return View("");
            }

            var Model = _Team.Entity.GetById(id);
            if (Model == null)
            {
                return View("ItemNotfound");
            }

            TeamMemberEditViewModel Member = new TeamMemberEditViewModel
            {
                Id = Model.Id,
               FullName = Model.FullName,
               TitleJob=Model.TitleJob,
                ImageUrl = Model.ImageUrl,
               FBUrllink =Model.FBUrllink,
               TWUrllink=Model.TWUrllink,
               LinkedInUrl=Model.LinkedInUrl
            };


            return View(Member);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TeamMemberEditViewModel model)
        {
            

            if (ModelState.IsValid)
            {
                
                    string ImageValue = model.ImageUrl;
                    if (model.File != null)
                    {
                        string uploads = Path.Combine(_hosting.WebRootPath, @"AdminRoot\img\Teammember");
                        string fullPath = Path.Combine(uploads, model.FullName + model.File.FileName);
                        ImageValue = model.File.FileName;
                        model.File.CopyTo(new FileStream(fullPath, FileMode.Create));

                    }

                    
                    TeamMember Member = new TeamMember
                    {
                        Id = model.Id,
                        FullName = model.FullName,
                        TitleJob = model.TitleJob,
                        ImageUrl = ImageValue ,
                        FBUrllink = model.FBUrllink,
                        TWUrllink = model.TWUrllink,
                        LinkedInUrl = model.LinkedInUrl
                    };


                    _Team.Entity.Update(Member);
                    _Team.Save();

                    return RedirectToAction("Index");
                }

                return View(model);
        }

    
        
        
        
        
        
        
        
        public ActionResult Delete(Guid Id)

        {    
                if (Id!=null)
                {
                    _Team.Entity.Delete(Id);
                    _Team.Save();
                    return RedirectToAction("Index");

            }
            return View("ItemNotfound");
        }
    }
}
