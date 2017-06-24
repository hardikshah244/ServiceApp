using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using ServiceApp.Domain.Security;
using ServiceApp.Domain.Concrete;
using ServiceApp.Domain.Entities;
using System.Web;

namespace ServiceApp.WebApi.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roleManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationRoleManager>();

            //var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();
            //var roleManager = context.OwinContext.GetUserManager<ApplicationRoleManager>();
            AuthRepository _repo = new AuthRepository(userManager, roleManager);
            ApplicationUser user = await _repo.FindUser(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }
            else if (user != null)
            {
                bool IsActive = _repo.CheckIsUserActiveOrDeactive(user.Email);

                if (IsActive == false)
                {
                    context.SetError("invalid_grant", "The user is deactivated.");
                    return;
                }
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("role", "user"));

            Dictionary<string, string> dicUserInfo = _repo.GetUserInfo(user);
            var props = new AuthenticationProperties(dicUserInfo);

            var ticket = new AuthenticationTicket(identity, props);

            context.Validated(ticket);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
    }
}