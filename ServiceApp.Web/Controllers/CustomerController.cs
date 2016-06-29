using ServiceApp.Domain.Abstract;
using ServiceApp.Domain.Security;
using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Linq;

namespace ServiceApp.Web.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        private ICustomerRepository _customerRepo = null;
        private IEngineerRepository _engineerRepo = null;

        public CustomerController(ICustomerRepository customerRepo, IEngineerRepository engineerRepo)
        {
            _customerRepo = customerRepo;
            _engineerRepo = engineerRepo;
        }

        // GET: Customer/Dashboard
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

                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        // POST: Customer/Dashboard
        [HttpPost]
        public ActionResult Dashboard(FormCollection ObjFormCollection)
        {
            try
            {
                int intCategory = Convert.ToInt16(ObjFormCollection["ddlCatregory"]);
                int intType = Convert.ToInt16(ObjFormCollection["ddlType"]);
                int intStatus = Convert.ToInt16(ObjFormCollection["ddlSatus"]);

                //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId());

                List<SelectListItem> lstCategory = (new SelectList(_engineerRepo.GetCategories(), "ServiceCategoryID", "ServiceCategoryName", intCategory)).ToList();
                lstCategory.Insert(0, (new SelectListItem { Text = "Select", Value = "0" }));
                ViewBag.Category = lstCategory;

                List<SelectListItem> lstType = (new SelectList(_engineerRepo.GetTypeMaster(), "ServiceTypeID", "ServiceTypeName", intType)).ToList();
                lstType.Insert(0, (new SelectListItem { Text = "Select", Value = "0" }));
                ViewBag.Type = lstType;

                List<SelectListItem> lstStatus = (new SelectList(_engineerRepo.GetStatus(), "StatusTypeID", "StatusTypeName", intStatus)).ToList();
                lstStatus.Insert(0, (new SelectListItem { Text = "Select", Value = "0" }));
                ViewBag.Satus = lstStatus;

                return PartialView("Dashboard", _engineerRepo.GetUserRequests("05d22bb6-28bd-4073-b261-42c5dfc7fede", intCategory, intType, intStatus));
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: Admin/Customer/ProfileInfo
        [HttpGet]
        public ActionResult ProfileInfo()
        {
            try
            {
                return View(_customerRepo.GetProfileInfo(User.Identity.Name));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}