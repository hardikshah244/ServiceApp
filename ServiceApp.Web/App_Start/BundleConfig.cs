using System.Web;
using System.Web.Optimization;

namespace ServiceApp.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //Scripts
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-2.1.3.min.js"));


            ////Lightbox 
            //bundles.Add(new StyleBundle("~/Content/lightbox").Include(
            //    "~/Content/nivo-lightbox.css",
            //    "~/Content/nivo_lightbox_themes/default/default.css"
            // ));          

        }
    }
}
