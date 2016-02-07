using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ServiceApp.Domain.Abstract;
using ServiceApp.Domain.Common;
using ServiceApp.Domain.Concrete;
using ServiceApp.Domain.DataModel;
using ServiceApp.Domain.Entities;
using ServiceApp.Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Security;

namespace ServiceApp.WebApi.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private AuthRepository _repo = null;
        private IUserDetailRepository _userRepo = null;

        public AccountController(IUserDetailRepository userRepo)
        {
            _repo = new AuthRepository();
            _userRepo = userRepo;
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(UserRegisterDetails userModel)
        {
            bool IsUserRegistered = false;
            ApplicationUser user = null;

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                IdentityResult result = await _repo.RegisterUser(userModel.User);

                if (result.Succeeded)
                {
                    user = await _repo.FindUser(userModel.User.Email, userModel.User.Password);

                    if (user != null)
                    {
                        UserDetail userDetail = new UserDetail
                        {
                            UserID = user.Id,
                            FirstName = userModel.UserDetails.FirstName,
                            LastName = userModel.UserDetails.LastName,
                            Latitude = userModel.UserDetails.Latitude,
                            Longitude = userModel.UserDetails.Longitude
                        };

                        _userRepo.AddUserDetail(userDetail);

                        IsUserRegistered = true;
                    }
                }

                IHttpActionResult errorResult = GetErrorResult(result);

                if (errorResult != null)
                {
                    return errorResult;
                }

                return Ok();
            }
            catch (Exception ex)
            {
                if(!IsUserRegistered)
                {
                    IdentityResult result = await _repo.DeleteUser(user);
                }

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
