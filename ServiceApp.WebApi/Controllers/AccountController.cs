using Microsoft.AspNet.Identity;
using ServiceApp.Domain.Common;
using ServiceApp.Domain.Concrete;
using ServiceApp.Domain.Entities;
using ServiceApp.Domain.Security;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Security;
using System.Web;
using Microsoft.AspNet.Identity.Owin;

namespace ServiceApp.WebApi.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private AuthRepository _repo = null;

        private ApplicationUserManager _userManager = null;
        private ApplicationRoleManager _userRoleManager = null;

        public AccountController()
        {
            _repo = new AuthRepository(AppUserManager, AppRoleManager);
        }

        protected ApplicationUserManager AppUserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        protected ApplicationRoleManager AppRoleManager
        {
            get
            {
                return _userRoleManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterUser userModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                IdentityResult result = await _repo.RegisterUser(userModel);

                if (result.Succeeded)
                {
                    return Ok("User successfully registered");
                }
                else
                {
                    IHttpActionResult errorResult = GetErrorResult(result);

                    if (errorResult != null)
                    {
                        return errorResult;
                    }

                    return BadRequest();
                }                
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(ex.Message, ex.InnerException));

                return BadRequest();
            }
        }

        // POST api/Account/ChangePassword
        [Authorize]
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePassword chnagePassword)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                IdentityResult result = await _repo.ChangePassword(chnagePassword);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(ex.Message, ex.InnerException));

                return BadRequest();
            }
        }

        [AllowAnonymous]
        [Route("ResetPassword")]
        public async Task<IHttpActionResult> ResetPassword(ResetPassword resetPassword)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                string strPassword = Membership.GeneratePassword(8, 0);
                IdentityResult result = null;

                if (!string.IsNullOrEmpty(strPassword))
                {
                    result = await _repo.ResetPassword(resetPassword, strPassword);
                }

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }
                else if (result.Succeeded)
                {
                    Email.SendEmail("hardik.shah.244@gmail.com", "hardik.shah.244@gmail.com", "ResetPassword", "Your password successfully reset. Please use below password to login <br/> Password : " + strPassword);
                }

                return Ok();

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(ex.Message, ex.InnerException));

                return BadRequest();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }

            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
