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
using ServiceApp.WebApi.Helpers;
using System.Configuration;
using System.Collections.Generic;

namespace ServiceApp.WebApi.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private AuthRepository _repo = null;
        private ApplicationUserManager _userManager = null;
        private ApplicationRoleManager _userRoleManager = null;

        private string strFromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();

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
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState.ValidateModelState());
                }

                IdentityResult result = _repo.RegisterUser(userModel);

                if (result.Succeeded)
                {
                    ResponseMessage = "User successfully registered";

                    Email.SendEmail(strFromEmail, userModel.Email, "RegisterUser", "You are successfully registered");

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

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("RegisterEngineer")]
        public HttpResponseMessage RegisterEngineer(RegisterEngineer userModel)
        {
            HttpResponseMessage ObjHttpResponseMessage = new HttpResponseMessage();
            string ResponseMessage = "";

            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState.ValidateModelState());
                }

                IdentityResult result = _repo.RegisterEngineer(userModel);

                if (result.Succeeded)
                {
                    ResponseMessage = "User successfully registered";

                    Email.SendEmail(strFromEmail, userModel.Email, "RegisterUser", "You are successfully registered");

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
        public HttpResponseMessage ChangePassword(ChangePassword chnagePassword)
        {
            HttpResponseMessage ObjHttpResponseMessage = new HttpResponseMessage();
            string ResponseMessage = "";
            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState.ValidateModelState());
                }

                IdentityResult result = _repo.ChangePassword(chnagePassword);

                if (!result.Succeeded)
                {
                    ResponseMessage = GetErrorResultStr(result);

                    if (!string.IsNullOrEmpty(ResponseMessage))
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessage);
                    }

                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error occurred on change password");
                }
                else if (result.Succeeded)
                {
                    Email.SendEmail(strFromEmail, chnagePassword.Email, "ChangePassword", "Your password successfully changed");
                }

                ResponseMessage = "Password successfully changed";
                return Request.CreateErrorResponse(HttpStatusCode.OK, ResponseMessage);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(ex.Message, ex.InnerException));

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Message :- " + ex.Message + "| InnerException :- " + ex.InnerException);
            }
        }

        [AllowAnonymous]
        [Route("ResetPassword")]
        public HttpResponseMessage ResetPassword(ResetPassword resetPassword)
        {
            HttpResponseMessage ObjHttpResponseMessage = new HttpResponseMessage();
            string ResponseMessage = "";

            try
            {
                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState.ValidateModelState());
                }

                string strPassword = Membership.GeneratePassword(8, 0);
                IdentityResult result = null;

                if (!string.IsNullOrEmpty(strPassword))
                {
                    result = _repo.ResetPassword(resetPassword, strPassword);
                }

                if (!result.Succeeded)
                {
                    ResponseMessage = GetErrorResultStr(result);

                    if (!string.IsNullOrEmpty(ResponseMessage))
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ResponseMessage);
                    }

                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error occurred on reset password");
                }
                else if (result.Succeeded)
                {
                    Email.SendEmail(strFromEmail, resetPassword.Email, "ResetPassword", "Your password successfully reset. Please use below password to login <br/> Password : " + strPassword);
                }

                ResponseMessage = "Password successfully reset";
                return Request.CreateErrorResponse(HttpStatusCode.OK, ResponseMessage);

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(ex.Message, ex.InnerException));

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Message :- " + ex.Message + "| InnerException :- " + ex.InnerException);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetUserInfoByServiceRequestID/{ServiceRequestID}")]
        public HttpResponseMessage GetUserInfoByServiceRequestID(int ServiceRequestID)
        {
            HttpResponseMessage ObjHttpResponseMessage = new HttpResponseMessage();
            try
            {
                if (ServiceRequestID > 0)
                {
                    Dictionary<string, string> dicUserInfo = _repo.GetUserInfoByServiceRequestID(ServiceRequestID);

                    if (dicUserInfo != null)
                        ObjHttpResponseMessage = Request.CreateResponse<Dictionary<string, string>>(HttpStatusCode.OK, dicUserInfo);
                    else
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data not found!");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Service request id not valid");
                }

                return ObjHttpResponseMessage;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(ex.Message, ex.InnerException));

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Message :- " + ex.Message + "| InnerException :- " + ex.InnerException);
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
