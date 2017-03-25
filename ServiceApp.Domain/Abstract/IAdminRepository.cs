using ServiceApp.Domain.DataModel;
using ServiceApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceApp.Domain.Abstract
{
    public interface IAdminRepository
    {
        AdminProfileInfo GetProfileInfo(string Email);

        IEnumerable<AdminUserMgtInfo> GetUserManagementInfo(string strEmailOrMobileNo);

        string UpdateUserActivateDeactivateByEmailOrPhone(bool strStatus, string strEmailOrMobileNo);

        IEnumerable<GET_ADMIN_CUST_DETAILS_Result> GetAdminCustomerDetails();

        IEnumerable<GET_ADMIN_ENGINEER_DETAILS_Result> GetAdminEngineerDetails();

        IEnumerable<GET_ADMIN_SR_MGT_Result> GetAdminSRMgtDetails();
        IEnumerable<GET_ADMIN_SR_RAISED_Result> GetAdminSRRaisedDetails();
        IEnumerable<GET_ADMIN_SR_ASSIGNED_Result> GetAdminSRAssignedDetails();
        IEnumerable<GET_ADMIN_SR_CLOSED_Result> GetAdminSRClosedDetails();
    }
}
