using ServiceApp.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceApp.Domain.DataModel;

namespace ServiceApp.Domain.Concrete
{
    public class EngineerRepository : IEngineerRepository, IDisposable
    {
        private ServiceAppDBContext context;

        public EngineerRepository()
        {
            this.context = new ServiceAppDBContext();
        }

        public IEnumerable<Engineer> GetEngineerDetailsByLocality(string Locality, string SubLocality, string City, string State, string Pincode, decimal Latitude, decimal Longitude)
        {
            try
            {
                IEnumerable<Engineer> lstEngineer = null;

                if (this.context != null)
                {
                    lstEngineer = (from engineer in context.Engineers
                                   where engineer.Locality == Locality && engineer.SubLocality == SubLocality
                                      && engineer.City == City && engineer.State == State && engineer.Pincode == Pincode
                                   select engineer).ToList();
                }

                return lstEngineer;
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
