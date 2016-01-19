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
                    IdentityUser user = await _repo.FindUser(userModel.User.UserName, userModel.User.Password);

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
            catch (Exception)
            {
                throw;
            }

            return Ok();
        }


        // POST api/Account/ChangePassword
        [AllowAnonymous]
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePassword chnagePassword)
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

            //Email.SendEmail("hardik.shah.244@gmail.com", "hardik.shah.244@gmail.com", "ChangePassword", "Your password successfully Changed!.");

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
