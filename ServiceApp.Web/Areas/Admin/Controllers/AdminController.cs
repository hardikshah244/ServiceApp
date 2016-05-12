using ServiceApp.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServiceApp.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private IAdminRepository _adminRepo = null;

        public AdminController(IAdminRepository adminRepo)
        {
            _adminRepo = adminRepo;
        }

        // GET: Admin/Admin/Dashboard
        [HttpGet]
        public ActionResult Dashboard()
        {
            return View();
        }

        // GET: Admin/Admin/ProfileInfo
        [HttpGet]
        public ActionResult ProfileInfo()
        {

            return View(_adminRepo.GetProfileInfo(User.Identity.Name));

        }
    }
}