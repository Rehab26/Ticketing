using System.Web;
using System.Web.Mvc;

namespace TicketingSystem.Web.Client
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomExceptionHandlerFilter());
        }
    }
}
