using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.ViewModels;

namespace Web.Controllers.Admin_Controller
{
    public class RolesController : Controller
    {

        private readonly IUnitOfWork<IdentityRole> _Roles;

        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly RoleManager<IdentityRole> _RoleManager;
        public RolesController(IUnitOfWork<IdentityRole> RoleManagers, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> _userManager)
        {
            _Roles = RoleManagers;
            _RoleManager = roleManager;
            _UserManager = _userManager;
        }


        // GET: RolesController
        public async Task<ActionResult> Index()
        {

            return View(_Roles.Entity.GetAll());
        }

        [HttpGet]
        public async Task<ActionResult> EditUserRoles(string roleId, string email)
        {

            ViewBag.roleId = roleId;

            var role = await _RoleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                return NotFound();
            }


            var model = new List<UserRoleViewModel>();
            var User = _UserManager.Users;
            if (email != null)
            {
                User = _UserManager.Users.Where(x => x.Email.Contains(email));
            }




            foreach (var user in User)
            {
                var userRoleViewModel = new UserRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.Email,
                    RoleId = role.Id
                };




                if (await _UserManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }







                model.Add(userRoleViewModel);
            }

            return View(model);



        }




        [HttpPost]
        public async Task<IActionResult> EditUserRoles(List<UserRoleViewModel> model)
        {

            var IdRole = "";
            for (int i = 0; i < model.Count; i++)
            {
                var role = await _RoleManager.FindByIdAsync(model[i].RoleId);
                IdRole = model[i].RoleId;
                ViewBag.roleId = IdRole;

                if (role == null)
                {

                    return NotFound();
                }


                var user = await _UserManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                if (model[i].IsSelected && !(await _UserManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _UserManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await _UserManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _UserManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditUserRoles", new { roleId = model[i].RoleId });
                }
            }

            return RedirectToAction("EditUserRoles", new { roleId = IdRole });
        }

        [HttpPost]
        public ActionResult SearchEmail(string email, string RoleId)
        {

            return RedirectToAction("EditUserRoles", new { roleId = RoleId, email = email });



        }








        // GET: RolesController/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View("CreateEditView");
        }

        [HttpPost]
        public ActionResult Create(string RoleName)

        {
            if (ModelState.IsValid &&RoleName!=null)
            {

                var value = _Roles.Entity.GetAll().Select(x => x.Id).LastOrDefault();

                var Role = new IdentityRole
                {
                    Id = value + 1 + RoleName,
                    Name = RoleName,
                    NormalizedName= RoleName

                };

                _Roles.Entity.Insert(Role);
                _Roles.Save();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }

    
        // POST: RolesController/Edit/5
        [HttpPost]
       
        public ActionResult Edit(string RoleName , string id)
        {
            if (ModelState.IsValid && RoleName != null)
            {

                var Role = _Roles.Entity.GetById(id);
                Role.Name = RoleName;
                Role.NormalizedName = RoleName;

               

                _Roles.Entity.Update(Role);
                _Roles.Save();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            if (id == null) { return NotFound(); }

            _Roles.Entity.Delete(id);
            _Roles.Save();
            return RedirectToAction("Index");


        }
    } 
}

