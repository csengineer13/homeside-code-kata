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
                    "~/bower_components/datatables.net/js/jquery.dataTables.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/application").Include(
                    "~/Scripts/app.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                    "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                    "~/bower_components/datatables.net-dt/css/jquery.dataTables.min.css",
                    "~/Content/site.css"));
        }
    }
}
