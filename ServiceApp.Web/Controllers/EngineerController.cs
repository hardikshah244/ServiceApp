using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ServiceApp.Domain.Abstract;
using ServiceApp.Domain.Entities;
using ServiceApp.Domain.Security;
using System;
using System.Web;
using System.Web.Mvc;
using ServiceApp.Web.Models;
using System.Collections.Generic;
using System.Linq;

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
        [HttpGet]
        public ActionResult Dashboard()
        {
            try
            {
                List<SelectListItem> lstCategory = (new SelectList(_engineerRepo.GetCategories(), "ServiceCategoryID", "ServiceCategoryName")).ToList();
                lstCategory.Insert(0, (new SelectListItem { Text = "Select", Value = "0" }));
                ViewBag.Category = lstCategory;

                List<SelectListItem> lstType = (new SelectList(_engineerRepo.GetTypeMaster(), "ServiceTypeID", "ServiceTypeName")).ToList();
                lstType.Insert(0, (new SelectListItem { Text = "Select", Value = "0" }));
                ViewBag.Type = lstType;

                List<SelectListItem> lstStatus = (new SelectList(_engineerRepo.GetStatus(), "StatusTypeID", "StatusTypeName")).ToList();
                lstStatus.Insert(0, (new SelectListItem { Text = "Select", Value = "0" }));
                ViewBag.Satus = lstStatus;

                //ViewBag.Category = new SelectList(_engineerRepo.GetCategories(), "ServiceCategoryID", "ServiceCategoryName");
                //ViewBag.Type = new SelectList(_engineerRepo.GetTypeMaster(), "ServiceTypeID", "ServiceTypeName");
                //ViewBag.Satus = new SelectList(_engineerRepo.GetStatus(), "StatusTypeID", "StatusTypeName");

                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        // POST: Engineer/Dashboard
        [HttpPost]
        public ActionResult Dashboard(FormCollection ObjFormCollection)
        {
            try
            {
                int intCategory = Convert.ToInt16(ObjFormCollection["ddlCatregory"]);
                int intType = Convert.ToInt16(ObjFormCollection["ddlType"]);
                int intStatus = Convert.ToInt16(ObjFormCollection["ddlSatus"]);

                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId());

                List<SelectListItem> lstCategory = (new SelectList(_engineerRepo.GetCategories(), "ServiceCategoryID", "ServiceCategoryName", intCategory)).ToList();
                lstCategory.Insert(0, (new SelectListItem { Text = "Select", Value = "0" }));
                ViewBag.Category = lstCategory;

                List<SelectListItem> lstType = (new SelectList(_engineerRepo.GetTypeMaster(), "ServiceTypeID", "ServiceTypeName", intType)).ToList();
                lstType.Insert(0, (new SelectListItem { Text = "Select", Value = "0" }));
                ViewBag.Type = lstType;

                List<SelectListItem> lstStatus = (new SelectList(_engineerRepo.GetStatus(), "StatusTypeID", "StatusTypeName", intStatus)).ToList();
                lstStatus.Insert(0, (new SelectListItem { Text = "Select", Value = "0" }));
                ViewBag.Satus = lstStatus;

                return View(_engineerRepo.GetEngineerRequests(user.Id, intCategory, intType, intStatus));
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