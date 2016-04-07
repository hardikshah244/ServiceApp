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
                //ServiceRequest ObjServiceRequest = new ServiceRequest();
                //ObjServiceRequest.ServiceTypeID = raiseRequest.ServiceTypeID;
                //ObjServiceRequest.StatusTypeID = raiseRequest.StatusTypeID;
                //ObjServiceRequest.Landmark = raiseRequest.Landmark;
                //ObjServiceRequest.Remark = raiseRequest.Remark;
                //ObjServiceRequest.CreatedUserID = raiseRequest.CreatedUserID;
                //ObjServiceRequest.CreatedDateTime = DateTime.Now;

                //context.ServiceRequests.Add(ObjServiceRequest);
                //context.SaveChanges();

                var GETENGINEERDETAILS_Result = context.GETENGINEERDETAILS(raiseRequest.ServiceTypeID, raiseRequest.StatusTypeID,
                                                                            raiseRequest.Landmark, raiseRequest.Remark, raiseRequest.CreatedUserID,
                                                                            Convert.ToInt32(raiseRequest.Pincode)).ToList();

                if (!string.IsNullOrEmpty((GETENGINEERDETAILS_Result[0]).Name) && !string.IsNullOrEmpty((GETENGINEERDETAILS_Result[0]).Email))
                {
                    ObjRaiseRequestResponse.ServiceRequestID = (GETENGINEERDETAILS_Result[0]).ServiceRequestID.GetValueOrDefault(0);
                    ObjRaiseRequestResponse.Name = (GETENGINEERDETAILS_Result[0]).Name;
                    ObjRaiseRequestResponse.Email = (GETENGINEERDETAILS_Result[0]).Email;
                    ObjRaiseRequestResponse.Message = "Success";

                }
                else
                {
                    ObjRaiseRequestResponse.ServiceRequestID = (GETENGINEERDETAILS_Result[0]).ServiceRequestID.GetValueOrDefault(0);
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

        public RequestResponse CancelRequestByEngineer(CancelRequest cancelRequest)
        {
            RequestResponse ObjRequestResponse = new RequestResponse();
            try
            {
                var RequestResult = context.ServiceRequests.FirstOrDefault(cr => cr.ServiceRequestID == cancelRequest.ServiceRequestID);

                if (RequestResult != null)
                {
                    RequestResult.StatusTypeID = cancelRequest.StatusTypeID;
                    RequestResult.ServiceRequestRemark = cancelRequest.ServiceRequestRemark;
                    RequestResult.UpdatedUserID = null;
                    RequestResult.UpdatedDateTime = DateTime.Now;

                    int Cnt = context.SaveChanges();

                    if (Cnt > 0)
                        ObjRequestResponse.Message = "Success";
                    else
                        ObjRequestResponse.Message = "Failed";
                }
                else
                {
                    ObjRequestResponse.Message = "Failed";
                }
            }
            catch (Exception)
            {
                throw;
            }

            return ObjRequestResponse;
        }

        public RequestResponse CloseRequestByEngineer(CloseRequest closeRequest)
        {
            RequestResponse ObjRequestResponse = new RequestResponse();

            try
            {
                var RequestResult = context.ServiceRequests.FirstOrDefault(cr => cr.ServiceRequestID == closeRequest.ServiceRequestID);

                if (RequestResult != null)
                {
                    RequestResult.StatusTypeID = closeRequest.StatusTypeID;
                    RequestResult.UpdatedDateTime = DateTime.Now;

                    int Cnt = context.SaveChanges();

                    if (Cnt > 0)
                        ObjRequestResponse.Message = "Success";
                    else
                        ObjRequestResponse.Message = "Failed";
                }
                else
                {
                    ObjRequestResponse.Message = "Failed";
                }
            }
            catch (Exception)
            {
                throw;
            }

            return ObjRequestResponse;
        }

        public RequestResponse CancelRequestByUser(CancelRequestByUser cancelRequestByUser)
        {
            RequestResponse ObjRequestResponse = new RequestResponse();
            try
            {
                var RequestResult = context.ServiceRequests.FirstOrDefault(cr => cr.ServiceRequestID == cancelRequestByUser.ServiceRequestID);

                if (RequestResult != null)
                {
                    RequestResult.StatusTypeID = cancelRequestByUser.StatusTypeID;
                    RequestResult.ServiceRequestRemark = cancelRequestByUser.ServiceRequestRemark;
                    RequestResult.UpdatedUserID = null;
                    RequestResult.UpdatedDateTime = DateTime.Now;
                    RequestResult.EngineerConfirmDateTime = null;

                    int Cnt = context.SaveChanges();

                    if (Cnt > 0)
                        ObjRequestResponse.Message = "Success";
                    else
                        ObjRequestResponse.Message = "Failed";
                }
                else
                {
                    ObjRequestResponse.Message = "Failed";
                }
            }
            catch (Exception)
            {
                throw;
            }

            return ObjRequestResponse;
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
