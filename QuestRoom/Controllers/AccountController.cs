using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using QuestRoom.Models;
using QuestRoom.Types;

namespace QuestRoom.Controllers
{
    public class AccountController : QuestRoomController
    {
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginViewModel{ Incorrect = false});
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToRoute("Default");
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            var user = Provider.GetUser(model.Username, model.Password);
            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(user.Name, false);
                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                    && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Info");
            }
            
            model.Incorrect = true;
            return View(model);
        }
    }
}