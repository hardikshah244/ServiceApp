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
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-2.1.3.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr.custom.js"));            

            bundles.Add(new ScriptBundle("~/bundles/themejs").Include(
                "~/Scripts/jquery.easing.min.js",       //easing 
                "~/Scripts/jquery.scrollTo.js",         //Scroll
                "~/Scripts/jquery.cycle.all.min.js",    //jQuery cycle
                "~/Scripts/jquery.form.js",   		  //jQuery form
                "~/Scripts/jquery.maximage.min.js",    //MaxImage background image slideshow
                "~/Scripts/materialize.js",    		  //Materialize
                "~/Scripts/classie.js",                //Class helper function
                "~/Scripts/pathLoader.js",             //Preloader Loading Animation
                "~/Scripts/preloader.js",              //Preloader
                "~/Scripts/count_down.js",             //Count Down Timer
                "~/Scripts/happy.js",                  //Newsletter form validation
                "~/Scripts/happy.methods.js",          //Newsletter form validation
                "~/Scripts/retina.js",                 //Retina.js - support for HiDPI Displays
                "~/Scripts/waypoints.min.js",          //Waypoints
                "~/Scripts/nivo-lightbox.min.js",      //Lightbox/Modalbox
                "~/Scripts/jquery.fitvids.js",     	  //Responsive Video
                "~/Scripts/jquery.stellar.js",     	  //Parallax backgrounds
                "~/Scripts/owl.carousel.js",     	  //Owl Carousel
                "~/Scripts/main.js",                 //Main
                "~/Scripts/mobile-navigation.js"                   //Mobile navigation
            ));


            
            //CSS Styles
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/grid12.css", //Grid
                "~/Content/typography.css", //Typography
                "~/Content/main.css" //Main
                ));

            bundles.Add(new StyleBundle("~/Content/colorscheme").Include(
               "~/Content/color_scheme_light.css", //Color scheme (dark/light)
               "~/Content/colors/color_palette_blue.css", //Color palette               
               "~/Content/rapid-icons.css" //Icons  
               ));

            bundles.Add(new StyleBundle("~/Content/slideshow").Include(
              "~/Content/jquery.maximage.min.css" //MaxImage background image slideshow              
              ));

            bundles.Add(new StyleBundle("~/Content/responsive").Include(
                "~/Content/responsivity.css", //responsive              
                "~/Content/animate.css"//animations
             ));

            //Lightbox 
            bundles.Add(new StyleBundle("~/Content/lightbox").Include(
                "~/Content/nivo-lightbox.css",
                "~/Content/nivo_lightbox_themes/default/default.css"
             ));

            //Owl - Carousel
            bundles.Add(new StyleBundle("~/Content/carousel").Include(
                "~/Content/owl.carousel.css",
                "~/Content/owl.theme.css",
                "~/Content/owl.transitions.css"
             ));

        }
    }
}
