using Microsoft.AspNet.Identity;
using ServiceApp.Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;
using ServiceApp.Domain.Security;
using System.Collections.Generic;
using ServiceApp.Domain.DataModel;
using System.Transactions;

namespace ServiceApp.Domain.Concrete
{
    public class AuthRepository : IDisposable
    {
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _userRoleManager;

        public AuthRepository(ApplicationUserManager UserManager, ApplicationRoleManager RoleManager)
        {
            _userManager = UserManager;
            _userRoleManager = RoleManager;
        }

        // In this application UserName consider as Email
        public IdentityResult RegisterUser(RegisterUser userModel)
        {
            IdentityResult result;
            ApplicationUser user = null;
            try
            {
                if (!ValidatePhoneNumberExists(userModel.PhoneNumber))
                {
                    user = new ApplicationUser
                    {
                        UserName = userModel.Email,
                        Email = userModel.Email,
                        PhoneNumber = userModel.PhoneNumber,
                        Name = userModel.Name,
                        Area = userModel.Area,
                        SubArea = userModel.SubArea,
                        City = userModel.City,
                        State = userModel.State,
                        Pincode = userModel.Pincode,
                        Latitude = userModel.Latitude,
                        Longitude = userModel.Longitude,
                        DeviceID = userModel.DeviceID
                    };

                    result = _userManager.Create(user, userModel.Password);

                    if (result.Succeeded)
                    {
                        //_userRoleManager.Create(new IdentityRole("Admin"));

                        if (_userRoleManager.RoleExists("Customer"))
                            _userManager.AddToRole(user.Id, "Customer");
                    }
                }
                else
                {
                    result = IdentityResult.Failed("Phone Number already exists");
                }

                return result;
            }
            catch (Exception)
            {
                _userManager.Delete(user);

                throw;
            }
        }

        // In this application UserName consider as Email
        public IdentityResult RegisterEngineer(RegisterEngineer userModel)
        {
            IdentityResult result;
            ApplicationUser user = null;
            try
            {
                if (userModel.MembershipType == "Paid")
                {
                    if (userModel.StartDate == null && userModel.Amount == 0)
                        return IdentityResult.Failed("Startdate can't be null or amount greater than zero");
                }

                if (!ValidatePhoneNumberExists(userModel.PhoneNumber))
                {
                    user = new ApplicationUser
                    {
                        UserName = userModel.Email,
                        Email = userModel.Email,
                        PhoneNumber = userModel.PhoneNumber,
                        Name = userModel.Name,
                        Area = userModel.Area,
                        SubArea = userModel.SubArea,
                        City = userModel.City,
                        State = userModel.State,
                        Pincode = userModel.Pincode,
                        Latitude = userModel.Latitude,
                        Longitude = userModel.Longitude,
                        DeviceID = userModel.DeviceID
                    };

                    result = _userManager.Create(user, userModel.Password);

                    if (result.Succeeded)
                    {
                        ServiceAppDBContext dbContext = new ServiceAppDBContext();

                        EngineerMembership ObjEngineerMembership = new EngineerMembership();
                        ObjEngineerMembership.MembershipType = userModel.MembershipType;
                        ObjEngineerMembership.CreationDate = DateTime.Now;
                        ObjEngineerMembership.UserId = user.Id;

                        if (ObjEngineerMembership.MembershipType == "Paid")
                        {
                            ObjEngineerMembership.StartDate = userModel.StartDate;
                            ObjEngineerMembership.EndDate = userModel.StartDate?.AddDays(30);
                            ObjEngineerMembership.Amount = userModel.Amount;
                        }

                        dbContext.EngineerMemberships.Add(ObjEngineerMembership);
                        dbContext.SaveChanges();

                        if (_userRoleManager.RoleExists("Engineer"))
                            _userManager.AddToRole(user.Id, "Engineer");
                    }
                }
                else
                {
                    result = IdentityResult.Failed("Phone Number already exists");
                }

                return result;
            }
            catch (Exception)
            {
                _userManager.Delete(user);

                throw;
            }
        }

        // In this application UserName consider as Email
        public IdentityResult ChangePassword(ChangePassword changePasswordModel)
        {
            try
            {
                IdentityResult result = null;

                ApplicationUser user = FindUserByEmail(changePasswordModel.Email);

                if (user != null)
                {
                    result = _userManager.ChangePassword(user.Id, changePasswordModel.OldPassword,
                    changePasswordModel.NewPassword);
                }

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IdentityResult ResetPassword(ResetPassword resetPasswordModel, string Password)
        {
            try
            {
                IdentityResult result = null;

                ApplicationUser user = FindUserByEmail(resetPasswordModel.Email);

                if (user != null)
                {
                    _userManager.RemovePassword(user.Id);

                    result = _userManager.AddPassword(user.Id, Password);
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
        public ApplicationUser FindUserByEmail(string Email)
        {
            try
            {
                ApplicationUser user = _userManager.FindByEmail(Email);

                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Dictionary<string, string> GetUserInfo(ApplicationUser user)
        {
            Dictionary<string, string> dicUserInfo = new Dictionary<string, string>();

            dicUserInfo.Add("Id", user.Id);
            dicUserInfo.Add("UserName", user.Name);
            dicUserInfo.Add("Email", user.Email);
            dicUserInfo.Add("PhoneNumber", user.PhoneNumber);
            dicUserInfo.Add("Name", user.Name);

            if (!string.IsNullOrEmpty(user.Address))
                dicUserInfo.Add("Address", user.Address);

            dicUserInfo.Add("Area", user.Area);
            dicUserInfo.Add("SubArea", user.SubArea);
            dicUserInfo.Add("City", user.City);
            dicUserInfo.Add("State", user.State);
            dicUserInfo.Add("Pincode", user.Pincode);
            dicUserInfo.Add("Role", GetUsersRole(user.Id));

            return dicUserInfo;
        }

        public Dictionary<string, string> GetUserInfoByServiceRequestID(int ServiceRequestID)
        {
            Dictionary<string, string> dicUserInfo = null;

            ServiceAppDBContext context = new ServiceAppDBContext();

            var RequestResult = (from user in context.AspNetUsers
                                 join servicerequest in context.ServiceRequests on user.Id equals servicerequest.CreatedUserID
                                 where servicerequest.ServiceRequestID == ServiceRequestID
                                 select new
                                 {
                                     user.Name,
                                     user.Email,
                                     user.PhoneNumber,
                                     user.Address,
                                     user.Area,
                                     user.SubArea,
                                     user.City,
                                     user.State,
                                     user.Pincode,
                                     servicerequest.Landmark,
                                     servicerequest.Remark
                                 }).FirstOrDefault();

            if (RequestResult != null)
            {
                dicUserInfo = new Dictionary<string, string>();

                dicUserInfo.Add("Name", RequestResult.Name);
                dicUserInfo.Add("Email", RequestResult.Email);
                dicUserInfo.Add("PhoneNumber", RequestResult.PhoneNumber);
                dicUserInfo.Add("Address", RequestResult.Address);
                dicUserInfo.Add("Area", RequestResult.Area);
                dicUserInfo.Add("SubArea", RequestResult.SubArea);
                dicUserInfo.Add("City", RequestResult.City);
                dicUserInfo.Add("State", RequestResult.State);
                dicUserInfo.Add("Pincode", RequestResult.Pincode);
                dicUserInfo.Add("Landmark", RequestResult.Landmark);
                dicUserInfo.Add("Remark", RequestResult.Remark);
            }

            return dicUserInfo;
        }

        public bool ValidatePhoneNumberExists(string PhoneNumber)
        {
            bool IsPhoneNumberExists = false;
            try
            {
                int PhoneNumberExistsCount = (from user in _userManager.Users
                                              where user.PhoneNumber == PhoneNumber
                                              select user.PhoneNumber).Count();

                if (PhoneNumberExistsCount > 0)
                    IsPhoneNumberExists = true;

                return IsPhoneNumberExists;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region RoleManager

        public string GetUsersRole(string userId)
        {
            var userRole = _userManager.GetRoles(userId).FirstOrDefault();

            return userRole;
        }

        #endregion

        public void Dispose()
        {
            _userManager.Dispose();
            _userRoleManager.Dispose();

        }
    }


}
