using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ServiceApp.Domain.Concrete;
using ServiceApp.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceApp.Web.Areas.Admin.Models;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Security;

namespace ServiceApp.Web.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        private AuthRepository _repo = null;
        private ApplicationUserManager _userManager = null;
        private ApplicationRoleManager _userRoleManager = null;

        public AccountController()
        {
            _repo = new AuthRepository(AppUserManager, AppRoleManager);
        }

        protected ApplicationUserManager AppUserManager
        {
            get
            {
                return _userManager ?? System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }

            private set { _userManager = value; }
        }

        protected ApplicationRoleManager AppRoleManager
        {
            get
            {
                return _userRoleManager ?? System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
            private set { _userRoleManager = value; }
        }

        // GET: Admin/Account/Login
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //Post : Admin/Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login loginModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (_userManager == null)
                    _userManager = AppUserManager;

                ApplicationUser user = _userManager.Find(loginModel.UserName, loginModel.Password);
                if (user != null)
                {
                    IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                    authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                    ClaimsIdentity identity = _userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationProperties props = new AuthenticationProperties();
                    props.IsPersistent = loginModel.RememberMe;
                    authenticationManager.SignIn(props, identity);

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            return View(loginModel);
        }

        //Post : Admin/Account/LogOut
        //[HttpPost]
        [Authorize]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            Session.Abandon();
            return RedirectToAction("Login", "Account", new { Area = "Admin" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgetPassword()
        {            
            var Email = Request.Form["Email"];

            if(!string.IsNullOrEmpty(Email))
            {
                if (_userManager == null)
                    _userManager = AppUserManager;

                string strPassword = Membership.GeneratePassword(8, 0);

                ApplicationUser user = _userManager.FindByEmail(Email);

                if (user != null)
                {
                    _userManager.RemovePassword(user.Id);

                    _userManager.AddPassword(user.Id, strPassword);
                }
            }
            else
            {
                ModelState.AddModelError("", "Enter email.");
            }

            return RedirectToAction("Login", "Account", new { Area = "Admin" });
        }
    }
}