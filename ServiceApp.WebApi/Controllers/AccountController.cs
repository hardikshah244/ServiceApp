using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ServiceApp.Domain.Abstract;
using ServiceApp.Domain.Common;
using ServiceApp.Domain.Entities;
using ServiceApp.WebApi.Models;
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
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                IdentityResult result = await _repo.RegisterUser(userModel.User);

                if (result.Succeeded)
                {
                    IdentityUser user = await _repo.FindUser(userModel.User.Email, userModel.User.Password);

                    if (user != null)
                    {
                        UserDetails userDetail = new UserDetails
                        {
                            UserID = user.Id,
                            FirstName = userModel.UserDetails.FirstName,
                            LastName = userModel.UserDetails.LastName,
                            Latitude = userModel.UserDetails.Latitude,
                            Longitude = userModel.UserDetails.Longitude
                        };

                        _userRepo.AddUserDetail(userDetail);
                    }
                }

                IHttpActionResult errorResult = GetErrorResult(result);

                if (errorResult != null)
                {
                    return errorResult;
                }

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(ex.Message, ex.InnerException));
            }

            return Ok();
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

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(ex.Message, ex.InnerException));
            }

            return Ok();
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

            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(ex.Message, ex.InnerException));
            }

            return Ok();
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
