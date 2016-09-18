using System.Web.Optimization;

namespace UMTD
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery-min").Include(
                        "~/Scripts/jquery-2.2.3.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jquery-ui-min").Include(
                        "~/Scripts/jquery-ui.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                        "~/Scripts/knockout-3.4.0.js"));
            bundles.Add(new ScriptBundle("~/bundles/test").Include(
                        "~/Scripts/test.js"));            

            bundles.Add(new ScriptBundle("~/bundles/helper").Include(
                        "~/Scripts/helper.js"));
            bundles.Add(new ScriptBundle("~/bundles/datamodel").Include(
                        "~/Scripts/data-model.js"));

            bundles.Add(new StyleBundle("~/Styles/jquery-ui-css").Include(
                      "~/Styles/jquery-ui.css"));
            bundles.Add(new StyleBundle("~/Styles/jquery-ui-struct-css").Include(
                      "~/Styles/jquery-ui.structure.css"));
            bundles.Add(new StyleBundle("~/Styles/jquery-ui-theme-css").Include(
                      "~/Styles/jquery-ui.theme.css"));
            bundles.Add(new StyleBundle("~/Styles/amazium").Include(
                      "~/Styles/amazium.css"));
            bundles.Add(new StyleBundle("~/Styles/site").Include(
                      "~/Styles/site.css"));
        }
    }
}