using ServiceApp.Domain.Abstract;
using System.Collections.Generic;
using System.Web.Mvc;
using ServiceApp.Domain.DataModel;
using System;

namespace ServiceApp.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private IAdminRepository _adminRepo = null;

        public AdminController(IAdminRepository adminRepo)
        {
            _adminRepo = adminRepo;
        }

        // GET: Admin/Dashboard
        [HttpGet]
        public ActionResult Dashboard()
        {
            var DashboardModels = new Tuple<IEnumerable<GET_ADMIN_SR_MGT_Result>, IEnumerable<GET_ADMIN_SR_RAISED_Result>,
                                        IEnumerable<GET_ADMIN_SR_ASSIGNED_Result>, IEnumerable<GET_ADMIN_SR_CLOSED_Result>>
                            (_adminRepo.GetAdminSRMgtDetails(), _adminRepo.GetAdminSRRaisedDetails(),
                            _adminRepo.GetAdminSRAssignedDetails(), _adminRepo.GetAdminSRClosedDetails());

            return View(DashboardModels);
        }

        // GET: Admin/ProfileInfo
        [HttpGet]
        public ActionResult ProfileInfo()
        {
            return View(_adminRepo.GetProfileInfo(User.Identity.Name));
        }

        // GET: Admin/UserManagement
        [HttpGet]
        public ActionResult UserManagement()
        {
            return View();
        }

        // POST: Admin/UserInfo
        [HttpPost]
        public ActionResult UserManagement(FormCollection ObjFormCollection)
        {
            string strEmailOrMobileNo = ObjFormCollection["txtUserMobileOrEmail"];

            return View("UserManagement", _adminRepo.GetUserManagementInfo(strEmailOrMobileNo));
        }

        // POST: Admin/UpdateUserActiveDeactive
        [HttpPost]
        public ActionResult UpdateUserActiveDeactive(bool ChkValue, string EmailOrMobileNo)
        {
            string strRetMessage = _adminRepo.UpdateUserActivateDeactivateByEmailOrPhone(ChkValue, EmailOrMobileNo);

            ViewBag.Message = strRetMessage;

            return View("UserManagement", _adminRepo.GetUserManagementInfo(EmailOrMobileNo));
        }

        // GET: Admin/CustomerDetails
        [HttpGet]
        public ActionResult CustomerDetails()
        {
            return View(_adminRepo.GetAdminCustomerDetails());
        }

        // GET: Admin/EngineerDetails
        [HttpGet]
        public ActionResult EngineerDetails()
        {
            return View(_adminRepo.GetAdminEngineerDetails());
        }
    }
}