using PagedList;
using ServiceApp.Domain.Abstract;
using ServiceApp.Domain.DataModel;
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

        // GET: Admin/EngineerInfo/Index
        [HttpGet]
        public ActionResult Index(string sortOrder, int page = 1, int pageSize = 5)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.EmailSortParm = String.IsNullOrEmpty(sortOrder) ? "desc" : "";

            var lstEngineerInfo = _engineerinfoRepo.GetEngineerInfo(sortOrder);

            PagedList<EngineerInfo> plEngineerInfo = new PagedList<EngineerInfo>(lstEngineerInfo, page, pageSize);

            return View(plEngineerInfo);
        }

        // GET: Admin/EngineerInfo/Create
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("_Create");
        }

        // POST: Admin/EngineerInfo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EngineerInfo engineerInfo)
        {
            if (ModelState.IsValid)
            {
                //db.People.Add(person);
                //await db.SaveChangesAsync();
                //return Json(new { success = true });
            }

            return PartialView("_Create", engineerInfo);
        }
    }
}