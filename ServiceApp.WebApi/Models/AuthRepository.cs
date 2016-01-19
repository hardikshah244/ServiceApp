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
            IdentityUser user = new IdentityUser
            {
                UserName = userModel.Email,
                Email = userModel.Email,
                PhoneNumber = userModel.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);

            return result;
        }

        // In this application UserName consider as Email
        public async Task<IdentityResult> ChangePassword(ChangePassword changePasswordModel)
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
        
        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await _userManager.FindAsync(userName, password);

            return user;
        }

        // In this application UserName consider as Email
        public async Task<IdentityUser> FindUserByName(string Email)
        {
            IdentityUser user = await _userManager.FindByNameAsync(Email);

            return user;
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }
    }
}