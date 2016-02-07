using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ServiceApp.Domain.DataModel;
using ServiceApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ServiceApp.Domain.Security;

namespace ServiceApp.Domain.Concrete
{
    public class AuthRepository : IDisposable
    {
        private OwinAuthDbContext _ctx;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _userRoleManager;

        public AuthRepository()
        {
            _ctx = new OwinAuthDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx));
            _userRoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_ctx));
        }

        // In this application UserName consider as Email
        public async Task<IdentityResult> RegisterUser(RegisterUser userModel)
        {
            try
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = userModel.Email,
                    Email = userModel.Email,
                    PhoneNumber = userModel.PhoneNumber,
                    BirthDate = Convert.ToDateTime(userModel.BirthDate)
                };

                var result = await _userManager.CreateAsync(user, userModel.Password);

                //if (result.Succeeded)
                //{
                //    _userRoleManager.Create(new IdentityRole("Admin"));

                //    if (_userRoleManager.RoleExists("Admin"))
                //        _userManager.AddToRole(user.Id, "Admin");
                //}

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

                ApplicationUser user = await FindUserByName(changePasswordModel.Email);

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

                ApplicationUser user = await FindUserByName(resetPasswordModel.Email);

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

        public async Task<ApplicationUser> FindUser(string userName, string password)
        {
            try
            {
                ApplicationUser user = await _userManager.FindAsync(userName, password);

                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IdentityResult> DeleteUser(ApplicationUser userModel)
        {
            try
            {
                IdentityResult userResult = await _userManager.DeleteAsync(userModel);

                return userResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // In this application UserName consider as Email
        public async Task<ApplicationUser> FindUserByName(string Email)
        {
            try
            {
                ApplicationUser user = await _userManager.FindByNameAsync(Email);

                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region RoleManager

        public IQueryable<ApplicationUser> GetUsersInRole(string userId)
        {
            return from user in _userManager.Users
                   where user.Roles.Any(r => r.UserId == userId)
                   select user;
        }

        #endregion

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }
    }


}
