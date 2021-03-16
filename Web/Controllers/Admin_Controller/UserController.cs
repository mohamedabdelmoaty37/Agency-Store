using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.ViewModels;

namespace Web.Controllers.Admin_Controller
{
    public class UserController : Controller
    {



        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUnitOfWork<ApplicationUser> _User;
        private readonly RoleManager<IdentityRole> _RoleManager;
        [Obsolete]
        private readonly IHostingEnvironment _hosting;
        private readonly IUnitOfWork<IdentityRole> _Role;

        [Obsolete]
        public UserController(IHostingEnvironment hosting, IUnitOfWork<IdentityRole> Role, RoleManager<IdentityRole> RoleManager, SignInManager<ApplicationUser> signInManager, IUnitOfWork<ApplicationUser> User, UserManager<ApplicationUser> userManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _User = User;
            _hosting = hosting;
            _RoleManager = RoleManager;
            _Role = Role;

        }


        public ActionResult Index()
        {
            return View(_User.Entity.GetAll());
        }

        // GET: UserController/Details/5
        public ActionResult Details(string id) 
        { 

            if (id == null)
                {
                    return NotFound();
                }

                var User = _User.Entity.GetById(id);
                if (User == null)
                {
                    return NotFound();
                }

               
            
            return View(User);
        }


        public ActionResult GetRoles(string id )

        {

            return View();
        }



        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string id)
        {
            ViewBag.userId = id;

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
               
                return NotFound();
            }

            var model = new List<UserRolesViewModel>();

            foreach (var role in _Role.Entity.GetAll())
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }

                model.Add(userRolesViewModel);
            }

            return View(model);
        }




        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
               
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }

            result = await _userManager.AddToRolesAsync(user,
                model.Where(x => x.IsSelected).Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }

            return RedirectToAction("Index");
        }





        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserviewModel Model)
        {
            if (ModelState.IsValid)
            {

                if (Model.File != null)
                {
                    string uploads = Path.Combine(_hosting.WebRootPath, @"img\ImageUsers");
                    string fullPath = Path.Combine(uploads, Model.FirstName + Model.LastName + Model.File.FileName);
                    Model.File.CopyTo(new FileStream(fullPath, FileMode.Create));
                }


                var user = new ApplicationUser
                {
                    UserName = Model.FirstName,
                    LastName = Model.LastName,
                    Address = Model.Address,
                    PhoneNumber = Model.phone,
                    Email = Model.Email,
                    PasswordHash = Model.Password,
                    Image = Model.FirstName + Model.LastName + Model.File.FileName

                };



                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)


                {
                    return RedirectToAction("Index");

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }


            return View(Model);
        }
    

        // GET: UserController/Edit/5
        public ActionResult Edit(string id)
        {

            if (id == null)
            {
                return View("");
            }

            var Model = _User.Entity.GetById(id);
            if (Model == null)
            {
                return NotFound();
            }

            UserviewModel user = new UserviewModel
            {
                FirstName = Model.UserName,
                LastName = Model.LastName,
                Address = Model.Address,
               phone = Model.PhoneNumber,
                Email = Model.Email,
                ConfirmPassword = Model.PasswordHash,
                Password = Model.PasswordHash,
              
                ImageUrl = Model.Image,
                Id=Model.Id


            };


            return View(user);
           
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Edit(UserviewModel Model)
        {
           
            if (ModelState.IsValid)
            {

                string ImageValue = Model.ImageUrl;
                if (Model.File != null)
                {
                    string uploads = Path.Combine(_hosting.WebRootPath, @"img\ImageUsers");
                    string fullPath = Path.Combine(uploads, Model.FirstName + Model.LastName + Model.File.FileName);
                    ImageValue = Model.FirstName + Model.LastName + Model.File.FileName;
                    Model.File.CopyTo(new FileStream(fullPath, FileMode.Create));
                }




             ApplicationUser Userr  = _User.Entity.GetById(Model.Id);

                Userr.UserName = Model.FirstName;
                Userr.LastName = Model.LastName;
                Userr.Address = Model.Address;
                Userr.PhoneNumber = Model.phone;
                Userr.Email = Model.Email;
                Userr.PasswordHash = Model.Password;
                Userr.Image = ImageValue;
                Userr.LockoutEnabled = true;
                Userr.AccessFailedCount = 0;
                Userr.PhoneNumberConfirmed = false;
                Userr.TwoFactorEnabled = false;
                Userr.EmailConfirmed = false;
                Userr.NormalizedEmail = Model.Email;

                _User.Entity.Update(Userr);
                _User.Save();

                return RedirectToAction("Index");


            }

            return View(Model);
        }

      

        
        public ActionResult Delete(string Id)

        {
            if (Id != null)
            {
                _User.Entity.Delete(Id);
                _User.Save();
                return RedirectToAction("Index");

            }
            return NotFound();
        }
    }
}
