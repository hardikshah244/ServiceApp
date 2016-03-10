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
using System.Net;

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
        public HttpResponseMessage Register(RegisterUser userModel)
        {
            HttpResponseMessage ObjHttpResponseMessage = new HttpResponseMessage();
            string ResponseMessage = "";

            try
            {
                if (!ModelState.IsValid)
                {
                    foreach (var state in ModelState)
                    {
                        if (!string.IsNullOrEmpty((state.Value.Errors[0]).ErrorMessage))
                            ResponseMessage += (state.Value.Errors[0]).ErrorMessage + "|";

                        if (!string.IsNullOrEmpty(((state.Value.Errors[0]).Exception).Message))
                            ResponseMessage += ((state.Value.Errors[0]).Exception).Message + "|";
                    }

                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessage.Substring(0, ResponseMessage.Length - 1));
                }

                IdentityResult result = _repo.RegisterUser(userModel);

                if (result.Succeeded)
                {
                    ResponseMessage = "User successfully registered";
                    return Request.CreateErrorResponse(HttpStatusCode.OK, ResponseMessage);
                }
                else
                {
                    ResponseMessage = GetErrorResultStr(result);

                    if (!string.IsNullOrEmpty(ResponseMessage))
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessage);
                    }

                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error occurred on register user");
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(ex.Message, ex.InnerException));

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Message :- " + ex.Message + "| InnerException :- " + ex.InnerException);
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

        private string GetErrorResultStr(IdentityResult result)
        {
            string ResponseMessage = "";

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ResponseMessage += error + "|";
                    }
                }

                return ResponseMessage.Substring(0, ResponseMessage.Length - 1);
            }

            return null;
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
