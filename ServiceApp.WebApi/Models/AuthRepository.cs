﻿using Microsoft.AspNet.Identity;
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

        public async Task<IdentityResult> RegisterUser(User userModel)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = userModel.UserName,
                Email = userModel.Email,
                PhoneNumber = userModel.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);                        

            return result;
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await _userManager.FindAsync(userName, password);

            return user;
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }
    }
}