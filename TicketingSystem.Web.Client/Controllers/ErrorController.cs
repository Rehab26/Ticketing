using System.Web.Mvc;

namespace TicketingSystem.Web.Client.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View("Error");
        }
    }
}