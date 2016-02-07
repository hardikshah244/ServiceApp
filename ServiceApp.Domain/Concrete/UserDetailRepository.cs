using ServiceApp.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceApp.Domain.Entities;
using ServiceApp.Domain.DataModel;

namespace ServiceApp.Domain.Concrete
{
    public class UserDetailRepository : IUserDetailRepository, IDisposable
    {
        private ServiceAppDBContext context;

        public UserDetailRepository()
        {
            this.context = new ServiceAppDBContext();
        }

        public void AddUserDetail(UserDetail userDetails)
        {
            try
            {
                if (this.context != null)
                {
                    this.context.UserDetails.Add(userDetails);
                    this.context.SaveChanges();
                }
            }
            catch (Exception ex)
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
