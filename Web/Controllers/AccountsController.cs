using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.ViewModels;

namespace Web.Controllers
{
    public class AccountsController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUnitOfWork<ApplicationUser> _User;
        [Obsolete]
        private readonly IHostingEnvironment _hosting;

        [Obsolete]
        public AccountsController(IHostingEnvironment hosting,SignInManager<ApplicationUser> signInManager, IUnitOfWork<ApplicationUser> User, UserManager<ApplicationUser> userManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _User = User;
            _hosting = hosting;

        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel Model)

        {

            if (ModelState.IsValid)
            {

                var user = _User.Entity.GetAll().Where(x => x.Email == Model.Email).SingleOrDefault();

                if (user != null)

                {
                    if (user.PasswordHash == Model.Password)

                    {
                        await _signInManager.SignInAsync(user, isPersistent: Model.RememberMe);
                        return RedirectToAction("index", "home");
                    }
                    else
                        ModelState.AddModelError(string.Empty, "Invalid Login Password Not Correct ");


                }


                else
                {

                    ModelState.AddModelError(string.Empty, "Invalid Login UserName Not Correct ");
                }
            }

            return View(Model);
        }



        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Obsolete]
        public async Task<IActionResult> Register(RegistervViewModel Model)

        {

            if (ModelState.IsValid)
            {

                if (Model.File != null)
                {
                    string uploads = Path.Combine(_hosting.WebRootPath, @"img\ImageUsers");
                    string fullPath = Path.Combine(uploads, Model.File.FileName);
                    Model.File.CopyTo(new FileStream(fullPath, FileMode.Create));
                }


                var user = new ApplicationUser {
                    UserName = Model.FirstName,
                    LastName=Model.LastName,
                    Address=Model.Address,
                    PhoneNumber = Model.phone,
                    Email = Model.Email,
                    PasswordHash = Model.Password ,
                    Image=Model.File.FileName
                
                };
               
                
                
                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)


                {
                    return RedirectToAction("Login");

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }


            return View(Model);
        }


        [HttpGet]
        public async Task <IActionResult> Logout()

        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");


        }






    }




}
