using Ninject;
using ServiceApp.Domain.Abstract;
using ServiceApp.Domain.Concrete;
using ServiceApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceApp.Web.Models
{
    public class Scheduler
    {
        public List<RaiseRequestResponse> GetAssignedEngrineerDetailsForScheduler()
        {
            List<RaiseRequestResponse> lstRaiseRequestResponse = null;
            try
            {
                IEngineerRepository _engineerRepo = new EngineerRepository();

                lstRaiseRequestResponse = _engineerRepo.GetEngineerForRequests();
            }
            catch (Exception)
            {
                throw;
            }

            return lstRaiseRequestResponse;
        }


    }
}