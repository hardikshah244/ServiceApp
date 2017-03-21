﻿using ServiceApp.Domain.Abstract;
using System.Web.Mvc;

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
            return View();
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
    }
}