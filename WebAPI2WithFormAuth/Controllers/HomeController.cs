﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebAPI2WithFormAuth.Models.ViewModels;

namespace WebAPI2WithFormAuth.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel login)
        {
            if (login.username == "admin" && login.password == "123")
            {
                FormsAuthentication.RedirectFromLoginPage(login.username, false);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
