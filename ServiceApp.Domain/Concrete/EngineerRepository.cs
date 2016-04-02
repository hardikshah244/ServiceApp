using ServiceApp.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceApp.Domain.DataModel;
using ServiceApp.Domain.Entities;

namespace ServiceApp.Domain.Concrete
{
    public class EngineerRepository : IEngineerRepository, IDisposable
    {
        private ServiceAppDBContext context;

        public EngineerRepository()
        {
            this.context = new ServiceAppDBContext();
        }

        public RaiseRequestResponse RaiseRequest(RaiseRequest raiseRequest)
        {
            RaiseRequestResponse ObjRaiseRequestResponse = new RaiseRequestResponse();
            try
            {
                ServiceRequest ObjServiceRequest = new ServiceRequest();
                ObjServiceRequest.ServiceTypeID = raiseRequest.ServiceTypeID;
                ObjServiceRequest.StatusTypeID = raiseRequest.StatusTypeID;
                ObjServiceRequest.Landmark = raiseRequest.Landmark;
                ObjServiceRequest.Remark = raiseRequest.Remark;
                ObjServiceRequest.CreatedUserID = raiseRequest.CreatedUserID;
                ObjServiceRequest.CreatedDateTime = DateTime.Now;

                context.ServiceRequests.Add(ObjServiceRequest);
                context.SaveChanges();

                var GETENGINEERDETAILS_Result = context.GETENGINEERDETAILS(Convert.ToInt32(raiseRequest.Pincode)).ToList();

                if (GETENGINEERDETAILS_Result.Count > 0)
                {
                    //if (!string.IsNullOrEmpty((GETENGINEERDETAILS_Result[0]).Name) && !string.IsNullOrEmpty((GETENGINEERDETAILS_Result[0]).Email))
                    ObjRaiseRequestResponse.ServiceRequestID = ObjServiceRequest.ServiceRequestID;
                    ObjRaiseRequestResponse.Name = (GETENGINEERDETAILS_Result[0]).Name;
                    ObjRaiseRequestResponse.Email = (GETENGINEERDETAILS_Result[0]).Email;
                    ObjRaiseRequestResponse.Message = "Success";

                }
                else
                {
                    ObjRaiseRequestResponse.ServiceRequestID = ObjServiceRequest.ServiceRequestID;
                    ObjRaiseRequestResponse.Name = "";
                    ObjRaiseRequestResponse.Email = "";
                    ObjRaiseRequestResponse.Message = "Failed";
                }
            }
            catch (Exception)
            {
                throw;
            }

            return ObjRaiseRequestResponse;
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
