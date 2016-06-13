using System.Web;
using System.Web.Optimization;

namespace CodeKata
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                    "~/bower_components/jquery/dist/jquery.min.js",
                    "~/bower_components/datatables.net/js/jquery.dataTables.min.js",
                    "~/bower_components/select2/dist/js/select2.min.js" // Full option available
                    ));

            bundles.Add(new ScriptBundle("~/bundles/application").Include(
                    "~/Scripts/app.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                    "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                    "~/bower_components/datatables.net-dt/css/jquery.dataTables.min.css",
                    "~/bower_components/select2/dist/css/select2.min.css",
                    "~/Toolkit/dist/assets/toolkit/styles/toolkit.css",
                    "~/Content/site.css"
                    ));
        }
    }
}
