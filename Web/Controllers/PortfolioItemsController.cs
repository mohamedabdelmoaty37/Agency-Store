using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Core.Entities;
using Infrastructure;
using Web.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Core.Interfaces;

namespace Web.Controllers
{
    public class PortfolioItemsController : Controller
    {
        private readonly IUnitOfWork<PortfolioItem> _portfolio;
        private readonly IUnitOfWork<Typecat> _Typecatogry;
        private readonly IHostingEnvironment _hosting;
        public List<GenderName> GenderName;


        public PortfolioItemsController(IUnitOfWork<PortfolioItem> portfolio, IUnitOfWork<Typecat> Typecatogry, IHostingEnvironment hosting)
        {
            _portfolio = portfolio;
            _hosting = hosting;
            _Typecatogry = Typecatogry;
            GenderName = new List<GenderName>()
            {   new GenderName { Value = "male", Name = "male" },
                new GenderName { Value = "female", Name = "female" },
                new GenderName { Value = "All Gender", Name = "All Gender" }
             };

        }

        // GET: PortfolioItems
        public IActionResult Index()
        {
            return View(_portfolio.Entity.GetAll());
        }

        // GET: PortfolioItems/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portfolioItem = _portfolio.Entity.GetById(id);
            if (portfolioItem == null)
            {
                return NotFound();
            }

            return View(portfolioItem);
        }


       

        // GET: PortfolioItems/Create
        public ActionResult Create()
        {
            

            ViewBag.GenderName = new SelectList(GenderName , "Value", "Name");


            ViewBag.CatogreName = new SelectList(_Typecatogry.Entity.GetAll(), "Id", "Typename", "typeId") ;



            return View();
        }

        // POST: TeamMemberController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PortfoliocreateViewModel Model)
        {

            if (ModelState.IsValid)
            {

                if (Model.File != null)
                {
                    string uploads = Path.Combine(_hosting.WebRootPath, @"AdminRoot\img\protofileitem");
                    string fullPath = Path.Combine(uploads, Model.Id+Model.NameCatogry + Model.File.FileName);
                    Model.File.CopyTo(new FileStream(fullPath, FileMode.Create));
                }


                var protofileitem = new PortfolioItem
                {
                    Price = Model.Price,
                    Company=Model.Company,
                    Namecatogry=Model.NameCatogry,
                    typeId=Model.typeId,
                    Title=Model.Title,
                    Date=Model.Date,
                    Description=Model.Description,
                    location=Model.location,
                    gendertype=Model.Gender,

                    ImageUrl = Model.Id + Model.NameCatogry + Model.File.FileName

                };



                _portfolio.Entity.Insert(protofileitem);
                _portfolio.Save();



                return RedirectToAction("Index");

            }


            ViewBag.GenderName = new SelectList(GenderName, "Value", "Name");


            ViewBag.CatogreName = new SelectList(_Typecatogry.Entity.GetAll(), "Id", "Typename", "typeId");


            return View(Model);
        }




















        // GET: PortfolioItems/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Model = _portfolio.Entity.GetById(id);
            if (Model == null)
            {
                return NotFound();
            }

           PortfoliEditViewModel portfolioViewModel = new PortfoliEditViewModel
           {
                Price = Model.Price,
                Company = Model.Company,
                NameCatogry = Model.Namecatogry,
                typeId = Model.typeId,
                Title = Model.Title,
                Date = Model.Date,
                Description = Model.Description,
                location = Model.location,
                Gender = Model.gendertype,
                ImageUrl = Model.ImageUrl

            };

            ViewBag.GenderName = new SelectList(GenderName, "Value", "Name");


            ViewBag.CatogreName = new SelectList(_Typecatogry.Entity.GetAll(), "Id", "Typename", "typeId");



            return View(portfolioViewModel);
        }

        // POST: PortfolioItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit( PortfoliEditViewModel Model)
        {
            if (ModelState.IsValid)
            {
                var ImageValue = Model.ImageUrl;
                if (Model.File != null)
                {
                    string uploads = Path.Combine(_hosting.WebRootPath, @"AdminRoot\img\protofileitem");
                    string fullPath = Path.Combine(uploads, Model.Id + Model.NameCatogry + Model.File.FileName);
                    ImageValue = Model.Id + Model.NameCatogry + Model.File.FileName;
                    Model.File.CopyTo(new FileStream(fullPath, FileMode.Create));
                }


                var protofileitem = new PortfolioItem
                {
                   Id=Model.Id,
                    Price = Model.Price,
                    Company = Model.Company,
                    Namecatogry = Model.NameCatogry,
                    typeId = Model.typeId,
                    Title = Model.Title,
                    Date = Model.Date,
                    Description = Model.Description,
                    location = Model.location,
                    gendertype = Model.Gender,

                    ImageUrl = ImageValue

                };



                _portfolio.Entity.Update(protofileitem);
                _portfolio.Save();



                return RedirectToAction("Index");

            }


            ViewBag.GenderName = new SelectList(GenderName, "Value", "Name");


            ViewBag.CatogreName = new SelectList(_Typecatogry.Entity.GetAll(), "Id", "Typename", "typeId");

            return View(Model);

        }

        // GET: PortfolioItems/Delete/5


        // POST: PortfolioItems/Delete/5


        public ActionResult Delete(Guid Id)

            {
                if (Id != null)
                {
                     _portfolio.Entity.Delete(Id);
                    _portfolio.Save();
                    return RedirectToAction("Index");

                }
                return View("ItemNotfound");
            }

           
           
        




        private bool PortfolioItemExists(Guid id)
        {
            return _portfolio.Entity.GetAll().Any(e => e.Id == id);
        }
    }
}
