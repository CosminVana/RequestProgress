using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace RequestProgress.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static ProgressTracker progressTracker;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            progressTracker = new ProgressTracker();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            progressTracker.TrackRequest(Request);
        }
    }
}
