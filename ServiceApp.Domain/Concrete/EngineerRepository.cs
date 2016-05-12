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
                    //RequestResult.UpdatedUserID = null;
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
                    //RequestResult.UpdatedUserID = null;
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

        public IEnumerable<UserRequestResponse> GetUserRequests(string CreatedUserID)
        {
            try
            {
                IEnumerable<UserRequestResponse> lstUserRequestResponse = (from SR in context.ServiceRequests
                                                                           join STM in context.ServiceTypeMasters on SR.ServiceTypeID equals STM.ServiceTypeID
                                                                           join STTM in context.StatusTypeMasters on SR.StatusTypeID equals STTM.StatusTypeID
                                                                           join USERS in context.AspNetUsers on SR.UpdatedUserID equals USERS.Id into CREATEDUSERS
                                                                           from USERSD in CREATEDUSERS.DefaultIfEmpty()
                                                                           where SR.CreatedUserID == CreatedUserID
                                                                           select new UserRequestResponse()
                                                                           {
                                                                               ServiceRequestID = SR.ServiceRequestID,
                                                                               CreatedDateTime = SR.CreatedDateTime,
                                                                               Landmark = SR.Landmark,
                                                                               Remark = SR.Remark,
                                                                               ServiceTypeName = STM.ServiceTypeName,
                                                                               StatusTypeName = STTM.StatusTypeName,
                                                                               Name = USERSD.Name,
                                                                               StatusTypeID = STTM.StatusTypeID
                                                                           }).AsEnumerable<UserRequestResponse>();

                return lstUserRequestResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<EngineerRequestResponse> GetEngineerRequests(string UpdatedUserID)
        {
            try
            {
                //IEnumerable<EngineerRequestResponse> lstEngineerRequestResponse = (from SR in context.ServiceRequests
                //                                                                   join STM in context.ServiceTypeMasters on SR.ServiceTypeID equals STM.ServiceTypeID
                //                                                                   join STTM in context.StatusTypeMasters on SR.StatusTypeID equals STTM.StatusTypeID
                //                                                                   join USERS in context.AspNetUsers on SR.CreatedUserID equals USERS.Id into CREATEDUSERS
                //                                                                   from USERSD in CREATEDUSERS.DefaultIfEmpty()
                //                                                                   where SR.UpdatedUserID == UpdatedUserID
                //                                                                   select new EngineerRequestResponse()
                //                                                                   {
                //                                                                       ServiceRequestID = SR.ServiceRequestID,
                //                                                                       CreatedDateTime = SR.CreatedDateTime,
                //                                                                       Landmark = SR.Landmark,
                //                                                                       Remark = SR.Remark,
                //                                                                       ServiceTypeName = STM.ServiceTypeName,
                //                                                                       StatusTypeName = STTM.StatusTypeName,
                //                                                                       Name = USERSD.Name,
                //                                                                       UpdatedDateTime = SR.UpdatedDateTime,
                //                                                                       StatusTypeID = STTM.StatusTypeID
                //                                                                   }).AsEnumerable<EngineerRequestResponse>();

                IEnumerable<EngineerRequestResponse> lstEngineerRequestResponse = context.GETENGINEERREQUESTS(UpdatedUserID).ToList<EngineerRequestResponse>();

                return lstEngineerRequestResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public EngineerProfileInfo GetProfileInfo(string Email)
        {
            try
            {
                EngineerProfileInfo ObjEngineerProfileInfo = (from USERINFO in context.AspNetUsers
                                                              join MEMBERSHIP in context.EngineerMemberships on USERINFO.Id equals MEMBERSHIP.UserId
                                                              where USERINFO.Email == Email
                                                              select new EngineerProfileInfo()
                                                              {
                                                                  Email = USERINFO.Email,
                                                                  Name = USERINFO.Name,
                                                                  PhoneNumber = USERINFO.PhoneNumber,
                                                                  City = USERINFO.City,
                                                                  State = USERINFO.State,
                                                                  Pincode = USERINFO.Pincode,
                                                                  MembershipType = MEMBERSHIP.MembershipType,
                                                                  StartDate = MEMBERSHIP.StartDate,
                                                                  EndDate = MEMBERSHIP.EndDate,
                                                                  Amount = MEMBERSHIP.Amount
                                                              }).FirstOrDefault<EngineerProfileInfo>();

                return ObjEngineerProfileInfo;
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
