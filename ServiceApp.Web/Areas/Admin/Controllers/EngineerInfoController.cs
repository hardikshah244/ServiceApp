using ServiceApp.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace ServiceApp.Web.Areas.Admin.Controllers
{
    public class EngineerInfoController : Controller
    {
        private IEngineerInfo _engineerinfoRepo = null;

        public EngineerInfoController(IEngineerInfo engineerinfoRepo)
        {
            _engineerinfoRepo = engineerinfoRepo;
        }

        // GET: Admin/EngineerInfo
        public ActionResult Index()
        {
            return View(_engineerinfoRepo.GetEngineerInfo());
        }
    }
}