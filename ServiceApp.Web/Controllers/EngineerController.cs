using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ServiceApp.Domain.Abstract;
using ServiceApp.Domain.Entities;
using ServiceApp.Domain.Security;
using System;
using System.Web;
using System.Web.Mvc;
using ServiceApp.Web.Models;

namespace ServiceApp.Web.Controllers
{
    [Authorize(Roles = "Engineer")]
    public class EngineerController : Controller
    {
        private IEngineerRepository _engineerRepo = null;

        public EngineerController(IEngineerRepository engineerRepo)
        {
            _engineerRepo = engineerRepo;
        }

        // GET: Engineer/Dashboard
        public ActionResult Dashboard()
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId());

                return View(_engineerRepo.GetEngineerRequests(user.Id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: Engineer/ProfileInfo
        [HttpGet]
        public ActionResult ProfileInfo()
        {
            try
            {
                return View(_engineerRepo.GetProfileInfo(User.Identity.Name));
            }
            catch (Exception)
            {
                throw;
            }
        }


        // GET: Engineer/Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultiButton(Name = "Save")]
        public ActionResult Save(EngineerProfileInfo engineerProfileInfo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    RequestResponse ObjRequestResponse = _engineerRepo.UpdateEngineerAddress(engineerProfileInfo.Email, engineerProfileInfo.Address);

                    if (ObjRequestResponse.Message == "Success")
                    {
                        ViewBag.Message = "Address successfully updated!";
                    }
                    else
                    {
                        ViewBag.Message = "Unable to update Address";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error occurs on updation info";

                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(ex.Message, ex.InnerException));
            }

            return PartialView("ProfileInfo", engineerProfileInfo);
        }

        // GET: Engineer/Cancel
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MultiButton(Name = "Cancel")]
        public ActionResult Cancel()
        {
            return PartialView("ProfileInfo", _engineerRepo.GetProfileInfo(User.Identity.Name));
        }
    }
}