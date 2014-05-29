using System.IO;
using System.Linq;
using System.Web.Mvc;
using StackTraceangelo.ProofOfConcept.Core;

namespace StackTraceangelo.ProofOfConcept.DotNet.AspDotNetArtViewer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var stackTraceArtClassWrappers = StackTraceArtClassWrapperHelper.FindStackTraceArtClassWrappersInDirectory(new DirectoryInfo(Server.MapPath("~/bin")));

            return View(stackTraceArtClassWrappers);
        }

        [HttpPost]
        public ActionResult DrawStackTraceArt(string stackTraceArtClassWrapperTypeFullName)
        {
            var stackTraceArtClassWrapperType = StackTraceArtClassWrapperHelper
                            .FindStackTraceArtClassWrappersInDirectory(new DirectoryInfo(Server.MapPath("~/bin")))
                            .Single(type => type.FullName == stackTraceArtClassWrapperTypeFullName);
            StackTraceArtClassWrapperHelper.Paint(stackTraceArtClassWrapperType);
            return null; // This will never happen because the above method throws the Stack Trace Art exception.
        }
    }
}