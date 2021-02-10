using System.Web.Mvc;

namespace TicketingSystem.Web.Client.Controllers
{
    public class HomeController : BaseController
    {

        public ActionResult Index()
        {
            return View();
        }
    }
}