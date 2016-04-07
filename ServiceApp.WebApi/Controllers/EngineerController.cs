using ServiceApp.Domain.Abstract;
using ServiceApp.Domain.DataModel;
using ServiceApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ServiceApp.WebApi.Controllers
{
    [RoutePrefix("api/Engineer")]
    //[Authorize]
    public class EngineerController : ApiController
    {
        private IEngineerRepository _engineerRepo = null;
        public EngineerController(IEngineerRepository engineerRepo)
        {
            _engineerRepo = engineerRepo;
        }

        [Route("RaiseRequest")]
        public HttpResponseMessage RaiseRequest(RaiseRequest raiseRequest)
        {
            HttpResponseMessage ObjHttpResponseMessage = new HttpResponseMessage();
            try
            {
                ObjHttpResponseMessage = Request.CreateResponse<RaiseRequestResponse>(HttpStatusCode.OK, _engineerRepo.RaiseRequest(raiseRequest));

                return ObjHttpResponseMessage;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(ex.Message, ex.InnerException));

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Message :- " + ex.Message + "| InnerException :- " + ex.InnerException);
            }
        }

        [Route("CancelRequest")]
        public HttpResponseMessage CancelRequestByEngineer(CancelRequest cancelRequest)
        {
            HttpResponseMessage ObjHttpResponseMessage = new HttpResponseMessage();

            try
            {
                ObjHttpResponseMessage = Request.CreateResponse<RequestResponse>(HttpStatusCode.OK, _engineerRepo.CancelRequestByEngineer(cancelRequest));

                return ObjHttpResponseMessage;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(ex.Message, ex.InnerException));

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Message :- " + ex.Message + "| InnerException :- " + ex.InnerException);
            }
        }

        [Route("CloseRequest")]
        public HttpResponseMessage CloseRequestByEngineer(CloseRequest closeRequest)
        {
            HttpResponseMessage ObjHttpResponseMessage = new HttpResponseMessage();

            try
            {
                ObjHttpResponseMessage = Request.CreateResponse<RequestResponse>(HttpStatusCode.OK, _engineerRepo.CloseRequestByEngineer(closeRequest));

                return ObjHttpResponseMessage;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(ex.Message, ex.InnerException));

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Message :- " + ex.Message + "| InnerException :- " + ex.InnerException);
            }
        }

        [Route("CancelRequestByUser")]
        public HttpResponseMessage CancelRequestByUser(CancelRequestByUser cancelRequestByUser)
        {
            HttpResponseMessage ObjHttpResponseMessage = new HttpResponseMessage();

            try
            {
                ObjHttpResponseMessage = Request.CreateResponse<RequestResponse>(HttpStatusCode.OK, _engineerRepo.CancelRequestByUser(cancelRequestByUser));

                return ObjHttpResponseMessage;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(ex.Message, ex.InnerException));

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Message :- " + ex.Message + "| InnerException :- " + ex.InnerException);
            }
        }
    }
}
