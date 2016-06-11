using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ServiceApp.Domain.Concrete;
using ServiceApp.Domain.Security;
using System;
using System.Web;
using System.Web.Mvc;
using ServiceApp.Web.Models;
using Microsoft.Owin.Security;
using System.Security.Claims;
using ServiceApp.Domain.Common;
using System.Configuration;

namespace ServiceApp.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private AuthRepository _repo = null;
        private ApplicationUserManager _userManager = null;
        private ApplicationRoleManager _userRoleManager = null;

        private string strFromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();

        public AccountController()
        {
            _repo = new AuthRepository(AppUserManager, AppRoleManager);

            //For Admin Users Creation
            //AdminCreation();
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
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        //Post : Admin/Account/Login        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login loginModel, string returnUrl)
        {
            try
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
                        //props.IsPersistent = loginModel.RememberMe;
                        authenticationManager.SignIn(props, identity);

                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            //if (User.IsInRole("Admin"))
                            //{
                            //    return RedirectToAction("Dashboard", "Admin");
                            //}
                            //else if (User.IsInRole("Customer"))
                            //{
                            //    return RedirectToAction("Dashboard", "Customer");
                            //}
                            //else if (User.IsInRole("Engineer"))
                            //{
                            //    return RedirectToAction("Dashboard", "Engineer");
                            //}

                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid username or password.");
                    }
                }

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(ex.Message, ex.InnerException));
            }

            return View(loginModel);
        }

        //Post : Admin/Account/LogOut
        [Authorize]
        public ActionResult LogOut()
        {
            try
            {
                IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                authenticationManager.SignOut();
                Session.Abandon();
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(ex.Message, ex.InnerException));
            }

            return RedirectToAction("Login", "Account");
        }

        // GET: Admin/Account/ForgotPassword
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View("ForgotPassword");
        }

        // POST: Admin/Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPassword forgotPassword)
        {
            try
            {
                string strEmail = forgotPassword.Email;
                IdentityResult result;

                if (ModelState.IsValid)
                {
                    if (_userManager == null)
                        _userManager = AppUserManager;

                    string strPassword = Validations.GeneratePassword(strEmail, "");

                    ApplicationUser user = _userManager.FindByEmail(strEmail);

                    if (user != null)
                    {
                        result = _userManager.RemovePassword(user.Id);

                        result = _userManager.AddPassword(user.Id, strPassword);

                        if (result.Succeeded)
                        {
                            ViewBag.Message = "Your password successfully reset!";

                            Email.SendEmail(strFromEmail, strEmail, "ForgotPassword", "Your password successfully reset, Please use below information for login <br/> Password :- " + strPassword);
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid email");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error occurs on forgot password");
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(ex.Message, ex.InnerException));
            }

            return View(forgotPassword);
        }

        // GET: Admin/Account/ChangePassword
        [Authorize]
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        // POST: Admin/Account/ChangePassword
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePassword chnagePassword)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_userManager == null)
                        _userManager = AppUserManager;

                    ApplicationUser user = _userManager.FindByEmail(User.Identity.Name);

                    if (user != null)
                    {
                        IdentityResult result = _userManager.ChangePassword(user.Id, chnagePassword.OldPassword,
                                                                chnagePassword.NewPassword);

                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error);
                            }
                        }
                        else if (result.Succeeded)
                        {
                            ViewBag.Message = "Your password successfully changed!";
                            Email.SendEmail(strFromEmail, User.Identity.Name, "ChangePassword", "Your password successfully changed");
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid old or new password.");
                    }
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(ex.Message, ex.InnerException));
            }

            return View(chnagePassword);
        }

        [NonAction]
        public void AdminCreation()
        {
            try
            {
                if (_userManager == null)
                    _userManager = AppUserManager;

                if (_userRoleManager == null)
                    _userRoleManager = AppRoleManager;

                var user = new ApplicationUser
                {
                    UserName = "Admin1@gmail.com",
                    Email = "Admin1@gmail.com",
                    PhoneNumber = "9913140245",
                    Name = "Admin1",
                    City = "Surat",
                    State = "Gujarat",
                    Pincode = "395005"
                };

                string strPassword = Validations.GeneratePassword(user.Email, user.PhoneNumber);

                var result = _userManager.Create(user, strPassword);

                if (result.Succeeded)
                {
                    if (_userRoleManager.RoleExists("Admin"))
                        _userManager.AddToRole(user.Id, "Admin");
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(ex.Message, ex.InnerException));
            }
        }
    }
}