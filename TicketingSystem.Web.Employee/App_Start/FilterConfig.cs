using System.Web;
using System.Web.Mvc;
using TicketingSystem.Web.Employee.Filters;

namespace TicketingSystem.Web.Employee
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
           // filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomExceptionHandlerFilter());
        }
    }
}
