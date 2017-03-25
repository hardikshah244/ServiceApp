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

        public string UpdateUserActivateDeactivateByEmailOrPhone(bool strStatus, string strEmailOrMobileNo)
        {
            string strRetMessage = "";

            var RequestResult = context.AspNetUsers.Where(U => U.Email == strEmailOrMobileNo).FirstOrDefault();

            if (RequestResult != null)
            {
                RequestResult.IsActive = strStatus;
                RequestResult.ModifiedDate = DateTime.Now;

                context.Entry(RequestResult).State = System.Data.Entity.EntityState.Modified;

                int Cnt = context.SaveChanges();

                if (Cnt > 0)
                    strRetMessage = "Record successfully updated";
                else
                    strRetMessage = "Unable to update record";
            }

            return strRetMessage;
        }

        public IEnumerable<GET_ADMIN_CUST_DETAILS_Result> GetAdminCustomerDetails()
        {
            return context.GET_ADMIN_CUST_DETAILS().ToList();
        }

        public IEnumerable<GET_ADMIN_ENGINEER_DETAILS_Result> GetAdminEngineerDetails()
        {
            return context.GET_ADMIN_ENGINEER_DETAILS().ToList();
        }

        public IEnumerable<GET_ADMIN_SR_MGT_Result> GetAdminSRMgtDetails()
        {
            return context.GET_ADMIN_SR_MGT().ToList();
        }

        public IEnumerable<GET_ADMIN_SR_RAISED_Result> GetAdminSRRaisedDetails()
        {
            return context.GET_ADMIN_SR_RAISED().ToList();
        }

        public IEnumerable<GET_ADMIN_SR_ASSIGNED_Result> GetAdminSRAssignedDetails()
        {
            return context.GET_ADMIN_SR_ASSIGNED().ToList();
        }

        public IEnumerable<GET_ADMIN_SR_CLOSED_Result> GetAdminSRClosedDetails()
        {
            return context.GET_ADMIN_SR_CLOSED().ToList();
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
