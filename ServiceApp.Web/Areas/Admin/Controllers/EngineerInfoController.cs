using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PagedList;
using ServiceApp.Domain.Abstract;
using ServiceApp.Domain.Common;
using ServiceApp.Domain.Concrete;
using ServiceApp.Domain.DataModel;
using ServiceApp.Domain.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ServiceApp.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EngineerInfoController : Controller
    {
        private IEngineerInfo _engineerinfoRepo = null;
        private AuthRepository _repo = null;
        private ApplicationUserManager _userManager = null;
        private ApplicationRoleManager _userRoleManager = null;

        private string strFromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();

        public EngineerInfoController(IEngineerInfo engineerinfoRepo)
        {
            _engineerinfoRepo = engineerinfoRepo;
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

        // GET: Admin/EngineerInfo/Index
        [HttpGet]
        public ActionResult Index(string sortOrder, int page = 1, int pageSize = 15)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.EmailSortParm = String.IsNullOrEmpty(sortOrder) ? "desc" : "";

            var lstEngineerInfo = _engineerinfoRepo.GetEngineerInfo(sortOrder);

            PagedList<EngineerInfo> plEngineerInfo = new PagedList<EngineerInfo>(lstEngineerInfo, page, pageSize);

            return View(plEngineerInfo);
        }

        // GET: Admin/EngineerInfo/Create
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("_Create");
        }

        // POST: Admin/EngineerInfo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EngineerInfo engineerInfo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string strPassword = Validations.GeneratePassword(engineerInfo.Email, engineerInfo.PhoneNumber);

                    IdentityResult result = _repo.RegisterEngineerWebApp(engineerInfo, strPassword);

                    if (result.Succeeded)
                    {
                        ViewBag.Message = "Engineer successfully registered";

                        Email.SendEmail(strFromEmail, engineerInfo.Email, "RegisterEngineer", "You are successfully registered in system, Please find below login information <br/>Email :- " + engineerInfo.Email + " Password:- " + strPassword);

                        return Json(new { success = true });
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error);
                        }
                    }
                }

            }
            catch (Exception)
            {
                ModelState.AddModelError("Error", "Error occurred on register engineer");
            }

            return PartialView("_Create", engineerInfo);
        }

    }
}