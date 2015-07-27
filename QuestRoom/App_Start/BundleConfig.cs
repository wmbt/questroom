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
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/common.js"));
            bundles.Add(new ScriptBundle("~/Scripts/confirm").Include(
                "~/Scripts/confirm.js"));
            bundles.Add(new ScriptBundle("~/Scripts/feedback").Include(
                "~/Scripts/feedback.js"));
            bundles.Add(new ScriptBundle("~/Scripts/bookingtable").Include(
                "~/Scripts/bookingtable.js"));
            bundles.Add(new ScriptBundle("~/Scripts/feedbacktable").Include(
                "~/Scripts/feedbacktable.js"));
            bundles.Add(new ScriptBundle("~/Scripts/schedule").Include(
                "~/Scripts/bootstrap-datepicker.js",
                "~/Scripts/bootstrap-datepicker.ru.min.js", 
                "~/Scripts/schedule.js"));
            /*bundles.Add(new StyleBundle("~/Content/common").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css"));*/
            bundles.Add(new StyleBundle("~/Content/schedule").Include(
                "~/Content/bootstrap-datepicker.css"));
            bundles.Add(new StyleBundle("~/Content/common").Include(
                "~/Content/bootstrap-slate.css",
                "~/Content/font-awesome.css",
                "~/Content/site.css"));
        }
    }
}