using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TicketingSystem.Web.Employee
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Ticket",
                url: "{controller}/{action}/{id}/{Title}",
                defaults: new { controller = "ticket", action = "ticket" },
                new[] { "TicketingSystem.Web.Employee.Controllers" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "UserLogin", id = UrlParameter.Optional },
                new[] { "TicketingSystem.Web.Employee.Controllers" }
            );
        }
    }
}
