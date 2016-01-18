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
            //try
            //{
            //    SqlConnection ObjSqlConnection = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=ServiceAppDB;Integrated Security=True");
            //    if (ObjSqlConnection.State != System.Data.ConnectionState.Open) ObjSqlConnection.Open();

            //    ViewBag.Title = "Home Page";
            //}
            //catch (Exception ex)
            //{
            //    ViewBag.Title = "Home Page" + ex.Message;
            //}

            return View();
        }
    }
}
