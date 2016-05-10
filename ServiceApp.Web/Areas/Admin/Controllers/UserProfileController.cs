using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServiceApp.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        // GET: Admin/UserProfile/Profile
        public ActionResult Profile1()
        {
            return View();
        }
    }
}