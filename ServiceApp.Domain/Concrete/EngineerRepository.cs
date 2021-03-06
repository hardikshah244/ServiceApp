﻿using ServiceApp.Domain.Abstract;
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

                var GETENGINEERDETAILS_Result = context.GETENGINEERDETAILS(raiseRequest.ServiceTypeID, raiseRequest.ServiceCategoryID, raiseRequest.StatusTypeID,
                                                                            raiseRequest.Landmark, raiseRequest.Remark, raiseRequest.CreatedUserID,
                                                                            Convert.ToInt32(raiseRequest.Pincode)).ToList();

                if (!string.IsNullOrEmpty((GETENGINEERDETAILS_Result[0]).Name) && !string.IsNullOrEmpty((GETENGINEERDETAILS_Result[0]).Email))
                {
                    ObjRaiseRequestResponse.ServiceRequestNO = (GETENGINEERDETAILS_Result[0]).ServiceRequestNO;
                    ObjRaiseRequestResponse.Name = (GETENGINEERDETAILS_Result[0]).Name;
                    ObjRaiseRequestResponse.Email = (GETENGINEERDETAILS_Result[0]).Email;
                    ObjRaiseRequestResponse.Message = "Success";

                }
                else
                {
                    ObjRaiseRequestResponse.ServiceRequestNO = (GETENGINEERDETAILS_Result[0]).ServiceRequestNO;
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
                var RequestResult = context.ServiceRequests.FirstOrDefault(cr => cr.ServiceRequestNO == cancelRequest.ServiceRequestNO);

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

        public RequestResponse AcceptRequestByEngineer(string ServiceRequestNO)
        {
            RequestResponse ObjRequestResponse = new RequestResponse();
            try
            {
                var RequestResult = context.ServiceRequests.FirstOrDefault(cr => cr.ServiceRequestNO == ServiceRequestNO);

                if (RequestResult != null)
                {
                    RequestResult.EngineerConfirmDateTime = DateTime.Now;

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
                var RequestResult = context.ServiceRequests.FirstOrDefault(cr => cr.ServiceRequestNO == closeRequest.ServiceRequestNO);

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
                var RequestResult = context.ServiceRequests.FirstOrDefault(cr => cr.ServiceRequestNO == cancelRequestByUser.ServiceRequestNO);

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

        public IEnumerable<UserRequestResponseAPI> GetUserRequests(string CreatedUserID)
        {
            try
            {
                IEnumerable<UserRequestResponseAPI> lstUserRequestResponse = (from SR in context.ServiceRequests
                                                                              join STM in context.ServiceTypeMasters on SR.ServiceTypeID equals STM.ServiceTypeID
                                                                              join SCM in context.ServiceCategoryMasters on SR.ServiceCategoryID equals SCM.ServiceCategoryID
                                                                              join STTM in context.StatusTypeMasters on SR.StatusTypeID equals STTM.StatusTypeID
                                                                              join USERS in context.AspNetUsers on SR.UpdatedUserID equals USERS.Id into CREATEDUSERS
                                                                              from USERSD in CREATEDUSERS.DefaultIfEmpty()
                                                                              where SR.CreatedUserID == CreatedUserID
                                                                              select new UserRequestResponseAPI()
                                                                              {
                                                                                  ServiceRequestNO = SR.ServiceRequestNO,
                                                                                  CreatedDateTime = SR.CreatedDateTime,
                                                                                  Landmark = SR.Landmark,
                                                                                  Remark = SR.Remark,
                                                                                  ServiceTypeName = STM.ServiceTypeName,
                                                                                  ServiceCategoryName = SCM.ServiceCategoryName,
                                                                                  StatusTypeName = STTM.StatusTypeName,
                                                                                  Name = USERSD.Name,
                                                                                  StatusTypeID = STTM.StatusTypeID
                                                                              }).AsEnumerable<UserRequestResponseAPI>();

                return lstUserRequestResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<UserRequestResponse> GetUserRequests(string CreatedUserID, int ServiceCategoryID, int ServiceTypeID, int StatusTypeID)
        {
            try
            {
                IEnumerable<UserRequestResponse> lstUserRequestResponse = context.GETUSERREQUESTS(CreatedUserID, ServiceCategoryID, ServiceTypeID, StatusTypeID).ToList<UserRequestResponse>();

                return lstUserRequestResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<EngineerRequestResponse> GetEngineerRequests(string UpdatedUserID, int ServiceCategoryID, int ServiceTypeID, int StatusTypeID)
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

                IEnumerable<EngineerRequestResponse> lstEngineerRequestResponse = context.GETENGINEERREQUESTS(UpdatedUserID, ServiceCategoryID, ServiceTypeID, StatusTypeID).ToList<EngineerRequestResponse>();

                return lstEngineerRequestResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<EngineerRequestResponseAPI> GetEngineerRequests(string UpdatedUserID)
        {
            try
            {
                IEnumerable<EngineerRequestResponseAPI> lstEngineerRequestResponse = context.GETENGINEERREQUESTS_API(UpdatedUserID).ToList<EngineerRequestResponseAPI>();

                return lstEngineerRequestResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<UserRequestResponseAPI> GetUserCurrentRequests(string CreatedUserID)
        {
            try
            {
                IEnumerable<UserRequestResponseAPI> lstUserRequestResponse = (from SR in context.ServiceRequests
                                                                              join STM in context.ServiceTypeMasters on SR.ServiceTypeID equals STM.ServiceTypeID
                                                                              join SCM in context.ServiceCategoryMasters on SR.ServiceCategoryID equals SCM.ServiceCategoryID
                                                                              join STTM in context.StatusTypeMasters on SR.StatusTypeID equals STTM.StatusTypeID
                                                                              join USERS in context.AspNetUsers on SR.UpdatedUserID equals USERS.Id into CREATEDUSERS
                                                                              from USERSD in CREATEDUSERS.DefaultIfEmpty()
                                                                              where SR.CreatedUserID == CreatedUserID
                                                                              && (SR.StatusTypeID == 1 || SR.StatusTypeID == 2)
                                                                              select new UserRequestResponseAPI()
                                                                              {
                                                                                  ServiceRequestNO = SR.ServiceRequestNO,
                                                                                  CreatedDateTime = SR.CreatedDateTime,
                                                                                  Landmark = SR.Landmark,
                                                                                  Remark = SR.Remark,
                                                                                  ServiceTypeName = STM.ServiceTypeName,
                                                                                  ServiceCategoryName = SCM.ServiceCategoryName,
                                                                                  StatusTypeName = STTM.StatusTypeName,
                                                                                  Name = USERSD.Name,
                                                                                  StatusTypeID = STTM.StatusTypeID
                                                                              }).AsEnumerable<UserRequestResponseAPI>();

                return lstUserRequestResponse;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<EngineerRequestResponseAPI> GetEngineerCurrentRequests(string UpdatedUserID)
        {
            try
            {
                IEnumerable<EngineerRequestResponseAPI> lstEngineerRequestResponse = (from SR in context.ServiceRequests
                                                                                      join STM in context.ServiceTypeMasters on SR.ServiceTypeID equals STM.ServiceTypeID
                                                                                      join SCM in context.ServiceCategoryMasters on SR.ServiceCategoryID equals SCM.ServiceCategoryID
                                                                                      join STTM in context.StatusTypeMasters on SR.StatusTypeID equals STTM.StatusTypeID
                                                                                      join USERS in context.AspNetUsers on SR.CreatedUserID equals USERS.Id into CREATEDUSERS
                                                                                      from USERSD in CREATEDUSERS.DefaultIfEmpty()
                                                                                      where SR.UpdatedUserID == UpdatedUserID
                                                                                          && (SR.StatusTypeID == 1 || SR.StatusTypeID == 2)
                                                                                      select new EngineerRequestResponseAPI()
                                                                                      {
                                                                                          ServiceRequestNO = SR.ServiceRequestNO,
                                                                                          ServiceCategoryName = SCM.ServiceCategoryName,
                                                                                          ServiceTypeName = STM.ServiceTypeName,
                                                                                          Name = USERSD.Name,
                                                                                          CreatedDateTime = SR.CreatedDateTime,
                                                                                          UpdatedDateTime = SR.UpdatedDateTime,
                                                                                          StatusTypeName = STTM.StatusTypeName,
                                                                                          StatusTypeID = STTM.StatusTypeID,
                                                                                          Landmark = SR.Landmark,
                                                                                          Remark = SR.Remark
                                                                                      }).AsEnumerable<EngineerRequestResponseAPI>();

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
                                                                  Address = USERINFO.Address,
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
        public RequestResponse UpdateEngineerAddress(string Email, string Address)
        {
            RequestResponse ObjRequestResponse = new RequestResponse();
            try
            {
                var RequestResult = context.AspNetUsers.FirstOrDefault(user => user.Email == Email);

                if (RequestResult != null)
                {
                    RequestResult.Address = Address.Trim();

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

        public IEnumerable<ServiceCategoryMaster> GetCategories()
        {
            try
            {
                IEnumerable<ServiceCategoryMaster> ObjServiceCategoryMaster = (from category in context.ServiceCategoryMasters
                                                                               select category).AsEnumerable();
                return ObjServiceCategoryMaster;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<ServiceTypeMaster> GetTypeMaster()
        {
            try
            {
                IEnumerable<ServiceTypeMaster> ObjServiceTypeMaster = (from type in context.ServiceTypeMasters
                                                                       select type).AsEnumerable();
                return ObjServiceTypeMaster;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<StatusTypeMaster> GetStatus()
        {
            try
            {
                IEnumerable<StatusTypeMaster> ObjStatusTypeMaster = (from status in context.StatusTypeMasters
                                                                     select status).AsEnumerable();
                return ObjStatusTypeMaster;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //For Scheduler
        public List<RaiseRequestResponse> GetEngineerForRequests()
        {
            List<RaiseRequestResponse> lstRaiseRequestResponse = null;
            try
            {
                IEnumerable<GETENGINEERS_SCHEDULER_Result> ObjGETENGINEERS_SCHEDULER = context.GETENGINEERS_SCHEDULER();

                lstRaiseRequestResponse = new List<RaiseRequestResponse>();

                foreach (var item in ObjGETENGINEERS_SCHEDULER)
                {
                    RaiseRequestResponse ObjRaiseRequestResponse = new RaiseRequestResponse();

                    var ASSIGNENGINEERS_SCHEDULER_Result = context.ASSIGNENGINEERS_SCHEDULER(item.ServiceRequestID, item.ServiceRequestNO, item.Pincode).ToList();

                    if (!string.IsNullOrEmpty((ASSIGNENGINEERS_SCHEDULER_Result[0]).Name) && !string.IsNullOrEmpty((ASSIGNENGINEERS_SCHEDULER_Result[0]).Email))
                    {
                        ObjRaiseRequestResponse.ServiceRequestNO = (ASSIGNENGINEERS_SCHEDULER_Result[0]).ServiceRequestNO;
                        ObjRaiseRequestResponse.Name = (ASSIGNENGINEERS_SCHEDULER_Result[0]).Name;
                        ObjRaiseRequestResponse.Email = (ASSIGNENGINEERS_SCHEDULER_Result[0]).Email;
                        ObjRaiseRequestResponse.Message = "Success";
                    }
                    else
                    {
                        ObjRaiseRequestResponse.ServiceRequestNO = (ASSIGNENGINEERS_SCHEDULER_Result[0]).ServiceRequestNO;
                        ObjRaiseRequestResponse.Name = "";
                        ObjRaiseRequestResponse.Email = "";
                        ObjRaiseRequestResponse.Message = "Failed";
                    }

                    lstRaiseRequestResponse.Add(ObjRaiseRequestResponse);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return lstRaiseRequestResponse;
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
