using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ServiceApp.Domain.Abstract;
using ServiceApp.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServiceApp.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Engineer")]
    public class EngineerController : Controller
    {
        private IEngineerRepository _engineerRepo = null;

        public EngineerController(IEngineerRepository engineerRepo)
        {
            _engineerRepo = engineerRepo;
        }

        // GET: Admin/Engineer/Dashboard
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

        // GET: Admin/Engineer/ProfileInfo
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
    }
}