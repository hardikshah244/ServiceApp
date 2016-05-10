using ServiceApp.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServiceApp.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        private ICustomerRepository _customerRepo = null;

        public CustomerController(ICustomerRepository customerRepo)
        {
            _customerRepo = customerRepo;
        }

        // GET: Admin/Customer/Dashboard
        public ActionResult Dashboard()
        {
            return View();
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