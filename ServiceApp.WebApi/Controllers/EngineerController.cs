using ServiceApp.Domain.Abstract;
using ServiceApp.Domain.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ServiceApp.WebApi.Controllers
{
    [RoutePrefix("api/Engineer")]
    [Authorize]
    public class EngineerController : ApiController
    {
        private IEngineerRepository _engineerRepo = null;
        public EngineerController(IEngineerRepository engineerRepo)
        {
            _engineerRepo = engineerRepo;
        }

        [Route("GetEngineerDetailsByLocality")]
        [HttpPost]
        public HttpResponseMessage GetEngineerDetailsByLocality(Engineer engineerModel)
        {
            //string Locality, string SubLocality, string City, string State, string Pincode, decimal Latitude, decimal Longitude   
            HttpResponseMessage ObjHttpResponseMessage = new HttpResponseMessage();
            try
            {
                IEnumerable<Engineer> lstEngineer = _engineerRepo.GetEngineerDetailsByLocality(engineerModel.Locality, engineerModel.SubLocality, engineerModel.City, engineerModel.State, engineerModel.Pincode, engineerModel.Latitude, engineerModel.Longitude);

                if (lstEngineer.Count() == 0)
                    ObjHttpResponseMessage = Request.CreateResponse<IEnumerable<Engineer>>(HttpStatusCode.NotFound, lstEngineer);
                else
                    ObjHttpResponseMessage = Request.CreateResponse<IEnumerable<Engineer>>(HttpStatusCode.OK, lstEngineer);
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(ex.Message, ex.InnerException));
                ObjHttpResponseMessage = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occurred on get engineer details");
            }
            return ObjHttpResponseMessage;
        }

    }
}
