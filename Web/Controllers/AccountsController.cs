using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
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
          public async Task<IActionResult> Login(string returnUrl)
            {
                LoginViewModel model = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins =
            (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            return View(model);
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
                    string fullPath = Path.Combine(uploads, Model.FirstName+Model.LastName+ Model.File.FileName);
                    Model.File.CopyTo(new FileStream(fullPath, FileMode.Create));
                }


                var user = new ApplicationUser {
                    UserName = Model.FirstName,
                    LastName=Model.LastName,
                    Address=Model.Address,
                    PhoneNumber = Model.phone,
                    Email = Model.Email,
                    PasswordHash = Model.Password ,
                    Image= Model.FirstName + Model.LastName + Model.File.FileName

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







        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Accounts",
                                new { ReturnUrl = returnUrl });
            var properties = _signInManager
                .ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }




        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            LoginViewModel loginViewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins =
                        (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState
                    .AddModelError(string.Empty, $"Error from external provider: {remoteError}");

                return View("Login", loginViewModel);
            }

            // Get the login information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState
                    .AddModelError(string.Empty, "Error loading external login information.");

                return View("Login", loginViewModel);
            }

            // If the user already has a login (i.e if there is a record in AspNetUserLogins
            // table) then sign-in the user with this external login provider


            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider,
                info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            // If there is no record in AspNetUserLogins table, the user may not have
            // a local account
            else
            {
                // Get the email claim value
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                if (email != null)
                {
                    // Create a new user without password if we do not have a user already
                    var user = await _userManager.FindByEmailAsync(email);

                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                            LastName = info.Principal.FindFirstValue(ClaimTypes.Name),

                        };

                        user.Address = "Egypt";
                        

                        await _userManager.CreateAsync(user);
                    }

                    // Add a login (i.e insert a row for the user in AspNetUserLogins table)
                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }

                // If we cannot find the user email we cannot continue
                ViewBag.ErrorTitle = $"Email claim not received from: {info.LoginProvider}";
                ViewBag.ErrorMessage = "Please contact support on Pragim@PragimTech.com";

                return View("Error");
            }
        }




    }






}
