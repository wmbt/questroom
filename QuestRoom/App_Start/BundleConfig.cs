using System.Web.Optimization;

namespace QuestRoom
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/common").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js"));
            
            bundles.Add(new StyleBundle("~/Content/common").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css"));
        }
    }
}