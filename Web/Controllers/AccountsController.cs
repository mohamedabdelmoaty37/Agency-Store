﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class AccountsController : Controller
    {
        public IActionResult Login ()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
        

    }
}
