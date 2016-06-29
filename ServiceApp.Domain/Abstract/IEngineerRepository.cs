using ServiceApp.Domain.DataModel;
using ServiceApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceApp.Domain.Abstract
{
    public interface IEngineerRepository : IDisposable
    {
        RaiseRequestResponse RaiseRequest(RaiseRequest raiseRequest);

        RequestResponse CancelRequestByEngineer(CancelRequest cancelRequest);

        RequestResponse CloseRequestByEngineer(CloseRequest closeRequest);

        RequestResponse CancelRequestByUser(CancelRequestByUser cancelRequestByUser);

        IEnumerable<UserRequestResponseAPI> GetUserRequests(string CreatedUserID);

        IEnumerable<UserRequestResponse> GetUserRequests(string CreatedUserID, int ServiceCategoryID, int ServiceTypeID, int StatusTypeID);

        IEnumerable<EngineerRequestResponse> GetEngineerRequests(string UpdatedUserID, int ServiceCategoryID, int ServiceTypeID, int StatusTypeID);

        IEnumerable<EngineerRequestResponseAPI> GetEngineerRequests(string UpdatedUserID);

        EngineerProfileInfo GetProfileInfo(string Email);

        RequestResponse UpdateEngineerAddress(string Email, string Address);

        IEnumerable<ServiceCategoryMaster> GetCategories();

        IEnumerable<ServiceTypeMaster> GetTypeMaster();

        IEnumerable<StatusTypeMaster> GetStatus();
    }
}

