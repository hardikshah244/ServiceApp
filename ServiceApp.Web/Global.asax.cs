using ServiceApp.Domain.Entities;
using ServiceApp.Web.Models;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ServiceApp.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        string strSchedulerURL = ConfigurationManager.AppSettings["SchedulerURL"];
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //for Development
            BundleTable.EnableOptimizations = false;
            //for Porduction
            //BundleTable.EnableOptimizations = true;

            Application["TotalVisitors"] = 0;

            //AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;

           // EngineerAssignmentRegisterCacheEntry();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //// If the dummy page is hit, then it means we want to add another item in cache
            //if (HttpContext.Current.Request.Url.ToString() == strSchedulerURL)
            //{
            //    // Add the item in cache and when succesful, do the work.
            //    EngineerAssignmentRegisterCacheEntry();
            //}
        }

        protected void Session_Start()
        {
            Application.Lock();
            Application["TotalVisitors"] = (int)Application["TotalVisitors"] + 1;
            Application.UnLock();
        }


        /// <summary>
        /// Register a cache entry which expires in 15 minute and gives us a callback.
        /// </summary>
        /// <returns></returns>
        private void EngineerAssignmentRegisterCacheEntry()
        {
            // Prevent duplicate key addition
            if (null != HttpContext.Current.Cache["PikPotEngineerAssignment"]) return;

            HttpContext.Current.Cache.Add("PikPotEngineerAssignment", "EngineerAssignmentValue", null, DateTime.MaxValue,
                TimeSpan.FromMinutes(2), CacheItemPriority.NotRemovable,
                new CacheItemRemovedCallback(CacheItemRemovedCallback));
        }

        /// <summary>
        /// Callback method which gets invoked whenever the cache entry expires.
        /// We can do our "service" works here.
        /// </summary>
        public void CacheItemRemovedCallback(
            string key,
            object value,
            CacheItemRemovedReason reason
            )
        {
            //Get Engineer which is free & Assign to customers
            GetEngineersAndAssignToCustomers();

            // We need to register another cache item which will expire again in one
            // minute. However, as this callback occurs without any HttpContext, we do not
            // have access to HttpContext and thus cannot access the Cache object. The
            // only way we can access HttpContext is when a request is being processed which
            // means a webpage is hit. So, we need to simulate a web page hit and then 
            // add the cache item.
            HitPage();
        }

        /// <summary>
        /// Hits a local webpage in order to add another expiring item in cache
        /// </summary>
        private void HitPage()
        {
            WebClient client = new WebClient();
            client.DownloadData(strSchedulerURL);
        }


        static void GetEngineersAndAssignToCustomers()
        {
            try
            {
                Scheduler ObjScheduler = new Scheduler();

                var AssignedEngrineerDetailsForScheduler = ObjScheduler.GetAssignedEngrineerDetailsForScheduler();

                if (AssignedEngrineerDetailsForScheduler != null)
                {
                    using (StreamWriter writer = new StreamWriter(@"F:\AssignedEngrineerDetailsForScheduler.txt", true))
                    {
                        writer.WriteLine("Start : {0}", DateTime.Now);
                        writer.WriteLine("======================================");

                        foreach (var item in AssignedEngrineerDetailsForScheduler)
                        {
                            writer.WriteLine("ServiceRequestNO : {0} || Name : {1} || Email : {2} || Message : {3}", item.ServiceRequestNO, item.Name, item.Email, item.Message);
                        }
                        writer.WriteLine("======================================");
                        writer.WriteLine("End : {0}", DateTime.Now);
                        writer.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
