using ServiceApp.Domain.Abstract;
using ServiceApp.Domain.DataModel;
using ServiceApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceApp.Domain.Concrete
{
    public class CustomerRepository : ICustomerRepository, IDisposable
    {
        private ServiceAppDBContext context;

        public CustomerRepository()
        {
            this.context = new ServiceAppDBContext();
        }

        public CustomerProfileInfo GetProfileInfo(string Email)
        {
            try
            {
                CustomerProfileInfo ObjCustomerProfileInfo = (from USERINFO in context.AspNetUsers
                                                              where USERINFO.Email == Email
                                                              select new CustomerProfileInfo()
                                                              {
                                                                  Email = USERINFO.Email,
                                                                  Name = USERINFO.Name,
                                                                  PhoneNumber = USERINFO.PhoneNumber,
                                                                  City = USERINFO.City,
                                                                  State = USERINFO.State,
                                                                  Area = USERINFO.Area,
                                                                  SubArea = USERINFO.SubArea,
                                                                  Address = USERINFO.Address,
                                                                  Pincode = USERINFO.Pincode
                                                              }).FirstOrDefault<CustomerProfileInfo>();

                return ObjCustomerProfileInfo;
            }
            catch (Exception)
            {
                throw;
            }
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
