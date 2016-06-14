using ServiceApp.Domain.Abstract;
using ServiceApp.Domain.Security;
using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

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

        // GET: Admin/Customer/Dashboard
        public ActionResult Dashboard()
        {
            try
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(User.Identity.GetUserId());

                return View(_engineerRepo.GetUserRequests(user.Id));
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