using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;

namespace ServiceApp.WebApi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //    try
            //    {
            //        SqlConnection ObjSqlConnection = new SqlConnection(@"Data Source=SQL5028.Smarterasp.net;Initial Catalog=DB_A098EE_ServiceAppDB;User Id=DB_A098EE_ServiceAppDB_admin;Password=hardikshah24488;");
            //        if (ObjSqlConnection.State != System.Data.ConnectionState.Open) ObjSqlConnection.Open();

            //        ViewBag.Title = "Home Page";
            //    }
            //    catch (Exception ex)
            //    {
            //        ViewBag.Title = "Home Page" + ex.Message;
            //    }

            return View();
        }
    }
}
