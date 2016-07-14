using ServiceApp.Domain.Abstract;
using ServiceApp.Domain.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceApp.Domain.Entities;

namespace ServiceApp.Domain.Concrete
{
    public class AdminRepository : IAdminRepository, IDisposable
    {
        private ServiceAppDBContext context;

        public AdminRepository()
        {
            this.context = new ServiceAppDBContext();
        }

        public AdminProfileInfo GetProfileInfo(string Email)
        {
            try
            {
                AdminProfileInfo ObjAdminProfileInfo = (from USERINFO in context.AspNetUsers
                                                        where USERINFO.Email == Email
                                                        select new AdminProfileInfo()
                                                        {
                                                            Email = USERINFO.Email,
                                                            Name = USERINFO.Name,
                                                            PhoneNumber = USERINFO.PhoneNumber,
                                                            City = USERINFO.City,
                                                            State = USERINFO.State,
                                                            Pincode = USERINFO.Pincode
                                                        }).FirstOrDefault<AdminProfileInfo>();

                return ObjAdminProfileInfo;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<AdminUserMgtInfo> GetUserManagementInfo(string strEmailOrMobileNo)
        {
            IEnumerable<AdminUserMgtInfo> lstAdminUserMgtInfo = context.GETADMINUSERMGTINFO(strEmailOrMobileNo).ToList<AdminUserMgtInfo>();

            return lstAdminUserMgtInfo;
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }

                disposed = true;
            }
        }

        public void Dispose() // Implement IDisposable
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
