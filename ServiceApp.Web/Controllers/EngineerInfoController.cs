using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PagedList;
using ServiceApp.Domain.Abstract;
using ServiceApp.Domain.Common;
using ServiceApp.Domain.Concrete;
using ServiceApp.Domain.DataModel;
using ServiceApp.Domain.Security;
using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace ServiceApp.Web.Controllers
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
        public ActionResult Index()
        {
            try
            {
                var lstEngineerInfo = _engineerinfoRepo.GetEngineerInfo();

                return View(lstEngineerInfo);
            }
            catch (Exception)
            {
                throw;
            }
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
        public ActionResult Create(EngineerInfo engineerInfo, HttpPostedFileBase file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string strPassword = Validations.GeneratePassword(engineerInfo.Email, engineerInfo.PhoneNumber);

                    IdentityResult result = _repo.RegisterEngineerWebApp(engineerInfo, strPassword);

                    if (result.Succeeded)
                    {
                        if (file.ContentLength > 0)
                        {
                            string strFileName = Path.GetFileName(file.FileName);
                            string strFileExtension = Path.GetExtension(file.FileName);
                            string strEngineerDocName = engineerInfo.PhoneNumber + strFileExtension;

                            string strPath = Path.Combine(Server.MapPath("~/UploadFiles"), strEngineerDocName);
                            file.SaveAs(strPath);
                        }

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
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "Error occurred on register engineer");
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(ex.Message, ex.InnerException));
            }

            return PartialView("_Create", engineerInfo);
        }

        [HttpPost]
        public ActionResult Index1(HttpPostedFileBase file)
        {

            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                file.SaveAs(path);
            }

            return RedirectToAction("Index1");
        }

    }
}