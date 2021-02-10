using System.Web.Mvc;

namespace TicketingSystem.Web.Employee.Controllers
{
    public class ErrorController : BaseController
    {
        
        public ActionResult Index()
        {
            return View("Error");
        }
    }
}