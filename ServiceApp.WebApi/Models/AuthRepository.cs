using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using ServiceApp.Domain.Abstract;
using ServiceApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;


namespace ServiceApp.WebApi.Models
{
    public class AuthRepository : IDisposable
    {
        private OwinAuthDbContext _ctx;
        private UserManager<IdentityUser> _userManager;

        public AuthRepository()
        {
            _ctx = new OwinAuthDbContext();
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_ctx));
        }

        // In this application UserName consider as Email
        public async Task<IdentityResult> RegisterUser(User userModel)
        {
            try
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = userModel.Email,
                    Email = userModel.Email,
                    PhoneNumber = userModel.PhoneNumber
                };

                var result = await _userManager.CreateAsync(user, userModel.Password);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // In this application UserName consider as Email
        public async Task<IdentityResult> ChangePassword(ChangePassword changePasswordModel)
        {
            try
            {
                IdentityResult result = null;

                IdentityUser user = await FindUserByName(changePasswordModel.Email);

                if (user != null)
                {
                    result = await _userManager.ChangePasswordAsync(user.Id, changePasswordModel.OldPassword,
                    changePasswordModel.NewPassword);
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IdentityResult> ResetPassword(ResetPassword resetPasswordModel, string Password)
        {
            try
            {
                IdentityResult result = null;

                IdentityUser user = await FindUserByName(resetPasswordModel.Email);

                if (user != null)
                {
                    _userManager.RemovePassword(user.Id);

                    result = await _userManager.AddPasswordAsync(user.Id, Password);
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            try
            {
                IdentityUser user = await _userManager.FindAsync(userName, password);

                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // In this application UserName consider as Email
        public async Task<IdentityUser> FindUserByName(string Email)
        {
            try
            {
                IdentityUser user = await _userManager.FindByNameAsync(Email);

                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }
    }
}