namespace Hangman.Web
{
    using System.Web.Optimization;

    public static class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterScripts(bundles);
            RegisterStyles(bundles);
        }

        private static void RegisterScripts(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js"));
            bundles.Add(new ScriptBundle("~/bundles/gridMvc").Include("~/Scripts/gridmvc.js"));
            bundles.Add(new ScriptBundle("~/bundles/signalr").Include("~/Scripts/jquery.signalR-2.2.0.js"));
        }

        private static void RegisterStyles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css")
                .Include(
                        "~/Content/bootstrap.css",
                        "~/Content/gridMvc.css",
                        "~/Content/site.css"));
        }
    }
}
