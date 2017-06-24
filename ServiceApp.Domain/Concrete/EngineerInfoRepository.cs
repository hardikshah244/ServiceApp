using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceApp.Domain.Abstract;
using ServiceApp.Domain.DataModel;
using ServiceApp.Domain.Entities;
using ServiceApp.Domain.Common;
using ServiceApp.Domain.Security;
using Microsoft.AspNet.Identity;


namespace ServiceApp.Domain.Concrete
{
    public class EngineerInfoRepository : IEngineerInfo, IDisposable
    {
        private ServiceAppDBContext context;

        public EngineerInfoRepository()
        {
            this.context = new ServiceAppDBContext();
        }

        public IEnumerable<EngineerInfo> GetEngineerInfo(string Email)
        {
            IEnumerable<EngineerInfo> lstEngineerInfo = context.GETENGINEERINFO(Email).ToList<EngineerInfo>();

            return lstEngineerInfo;
        }

        public EngineerInfo GetEngineerInfoByEmail(string Email)
        {
            EngineerInfo lstEngineerInfo = context.GETENGINEERINFO(Email).First();

            return lstEngineerInfo;
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
