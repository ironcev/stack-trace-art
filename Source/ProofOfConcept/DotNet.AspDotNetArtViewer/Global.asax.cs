using System.Web.Mvc;
using System.Web.Routing;

namespace StackTraceangelo.ProofOfConcept.DotNet.AspDotNetArtViewer
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
