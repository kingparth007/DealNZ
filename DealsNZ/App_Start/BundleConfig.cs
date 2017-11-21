using System.Web;
using System.Web.Optimization;

namespace DealsNZ
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/materializejs").Include(
                       "~/Content/js/init.js"));
            
            bundles.Add(new StyleBundle("~/bundles/materializecss").Include(
                "~/Content/css/materialize.css",
                "~/Content/css/style.css"));
        }
    }
}
