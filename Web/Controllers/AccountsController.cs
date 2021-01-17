using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.ViewModels;

namespace Web.Controllers
{
    public class AccountsController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUnitOfWork<IdentityUser> _User;

        public AccountsController(SignInManager<IdentityUser> signInManager, IUnitOfWork<IdentityUser> User, UserManager<IdentityUser> userManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _User = User;
        }


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
        public async Task<IActionResult> Register(RegistervViewModel Model)

        {

            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Model.UserName, PhoneNumber = Model.phone, Email = Model.Email, PasswordHash = Model.Password };
                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)


                {


                    return RedirectToAction("Accounts", "Login");



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
