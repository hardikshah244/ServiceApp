using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ServiceApp.Domain.Abstract;
using ServiceApp.Domain.Common;
using ServiceApp.Domain.Concrete;
using ServiceApp.Domain.DataModel;
using ServiceApp.Domain.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.IO.Compression;
using System.Linq;
using System.Net;

namespace ServiceApp.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EngineerInfoController : Controller
    {
        private IEngineerInfo _engineerinfoRepo = null;
        private AuthRepository _repo = null;
        private ApplicationUserManager _userManager = null;
        private ApplicationRoleManager _userRoleManager = null;

        private string strFromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();
        private string strDocumentUploadPath = ConfigurationManager.AppSettings["DocumentUploadPath"].ToString();
        private string strDocumentDownloadFiles = ConfigurationManager.AppSettings["DocumentDownloadFiles"].ToString();

        public EngineerInfoController(IEngineerInfo engineerinfoRepo)
        {
            _engineerinfoRepo = engineerinfoRepo;
            _repo = new AuthRepository(AppUserManager, AppRoleManager);
        }

        protected ApplicationUserManager AppUserManager
        {
            get
            {
                return _userManager ?? System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }

            private set { _userManager = value; }
        }

        protected ApplicationRoleManager AppRoleManager
        {
            get
            {
                return _userRoleManager ?? System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
            private set { _userRoleManager = value; }
        }

        // GET: Admin/EngineerInfo/Index
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                var lstEngineerInfo = _engineerinfoRepo.GetEngineerInfo("");

                return View(lstEngineerInfo);
            }
            catch (Exception)
            {
                throw;
            }
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
            try
            {
                if (ModelState.IsValid)
                {
                    List<string> fileDetails = new List<string>();

                    if (Request.Files != null && Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                    {
                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            var file = Request.Files[i];

                            if (file != null && file.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(file.FileName);
                                var FileExtension = Path.GetExtension(file.FileName);
                                var FileDirectory = Server.MapPath(strDocumentUploadPath + engineerInfo.PhoneNumber);

                                if (!Directory.Exists(FileDirectory))
                                {
                                    Directory.CreateDirectory(FileDirectory);
                                }

                                var DocNameWithDirectory = Path.Combine(FileDirectory, fileName);

                                if (!System.IO.File.Exists(DocNameWithDirectory))
                                {
                                    file.SaveAs(DocNameWithDirectory);

                                    fileDetails.Add(fileName);
                                }
                                else
                                {
                                    ModelState.AddModelError("Error", "Document " + fileName + " already available");
                                }
                            }
                        }

                        string strPassword = Validations.GeneratePassword(engineerInfo.Email, engineerInfo.PhoneNumber);

                        IdentityResult result = _repo.RegisterEngineerWebApp(engineerInfo, strPassword);

                        if (result.Succeeded)
                        {
                            engineerInfo.FileDetails = fileDetails;

                            ViewBag.Message = "Engineer successfully registered";

                            Email.SendEmail(strFromEmail, engineerInfo.Email, "RegisterEngineer", "You are successfully registered in system, Please find below login information <br/>Email :- " + engineerInfo.Email + " Password:- " + strPassword);

                            return Json(new { success = true, Message = "Engineer successfully registered" });
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "Please upload atleast 1 document");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "Error occurred on register engineer");
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(ex.Message, ex.InnerException));
            }

            return PartialView("_Create", engineerInfo);
        }

        // GET: Admin/EngineerInfo/Edit
        [HttpGet]
        public ActionResult Edit(string Email)
        {
            try
            {
                var lstEngineerInfo = _engineerinfoRepo.GetEngineerInfoByEmail(Email);

                string strUploadedFileDirectory = Server.MapPath(strDocumentUploadPath + lstEngineerInfo.PhoneNumber);
                List<string> lstDocuments = null;
                if (Directory.Exists(strUploadedFileDirectory))
                {
                    DirectoryInfo di = new DirectoryInfo(strUploadedFileDirectory);

                    lstDocuments = di.GetFiles().Select(file => file.Name).ToList();
                }

                lstEngineerInfo.FileDetails = lstDocuments;

                return PartialView("_Edit", lstEngineerInfo);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // POST: Admin/EngineerInfo/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EngineerInfo engineerInfo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    List<string> fileDetails = (engineerInfo.FileDetails == null || engineerInfo.FileDetails[0] == "") ? null : engineerInfo.FileDetails[0].Split(',').ToList<string>();

                    if ((Request.Files != null && Request.Files.Count > 0 && Request.Files[0].ContentLength > 0) || (fileDetails != null && fileDetails.Count > 0))
                    {
                        for (int i = 0; i < Request.Files.Count; i++)
                        {
                            var file = Request.Files[i];

                            if (file != null && file.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(file.FileName);
                                var FileDirectory = Server.MapPath(strDocumentUploadPath + engineerInfo.PhoneNumber);

                                if (!Directory.Exists(FileDirectory))
                                {
                                    Directory.CreateDirectory(FileDirectory);
                                }

                                var DocNameWithDirectory = Path.Combine(FileDirectory, fileName);

                                if (!System.IO.File.Exists(DocNameWithDirectory))
                                {
                                    file.SaveAs(DocNameWithDirectory);

                                    fileDetails.Add(fileName);
                                }
                                else
                                {
                                    ModelState.AddModelError("Error", "Document " + fileName + " already available");
                                }
                            }
                        }

                        IdentityResult result = _repo.UpdateEngineerWebApp(engineerInfo);

                        if (result.Succeeded)
                        {
                            engineerInfo.FileDetails = fileDetails;

                            ViewBag.Message = "Engineer Information updated successfully";

                            // Email.SendEmail(strFromEmail, engineerInfo.Email, "RegisterEngineer", "You are successfully registered in system, Please find below login information <br/>Email :- " + engineerInfo.Email + " Password:- " + strPassword);

                            return Json(new { success = true, Message = "Engineer Information updated successfully" });
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "Please upload atleast 1 document");
                    }
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "Error occurred on update engineer inforamtion");
                Elmah.ErrorSignal.FromCurrentContext().Raise(new Exception(ex.Message, ex.InnerException));
            }

            return PartialView("_Edit", engineerInfo);
        }

        [HttpGet]
        public ActionResult DownloadZipDoc(string DocId)
        {
            string strArchive = Server.MapPath(strDocumentDownloadFiles + DocId + ".zip");
            try
            {
                string strUploadedFileDirectory = Server.MapPath(strDocumentUploadPath + DocId);

                if (Directory.Exists(strUploadedFileDirectory))
                {
                    // Delete existing .Zip file
                    if (System.IO.File.Exists(strArchive))
                    {
                        System.IO.File.Delete(strArchive);
                    }

                    // Create .Zip file from Uploaded files
                    ZipFile.CreateFromDirectory(strUploadedFileDirectory, strArchive, CompressionLevel.Fastest, true);

                    return File(strArchive, "application/zip", DocId + ".zip");
                }
            }
            catch (Exception)
            {
                return Content("Error occurred on download files");
            }

            return RedirectToAction("Index");
        }

        public FileResult DownloadDoc(string Doc, string FileName)
        {
            return File(Path.Combine(Server.MapPath(strDocumentUploadPath) + Doc, FileName), System.Net.Mime.MediaTypeNames.Application.Octet, FileName);
        }

        [HttpPost]
        public JsonResult DeleteDoc(string Doc, string FileName)
        {
            if (string.IsNullOrEmpty(Doc) && string.IsNullOrEmpty(FileName))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Result = "Error" });
            }
            try
            {
                //Delete document from the file system
                string strDocName = Path.Combine(Server.MapPath(strDocumentUploadPath) + Doc, FileName);
                if (System.IO.File.Exists(strDocName))
                {
                    System.IO.File.Delete(strDocName);
                }
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}