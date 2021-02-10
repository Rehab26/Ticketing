using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TicketingSystem.Web.Client.Models;

namespace TicketingSystem.Web.Client.Filters
{
    public class CustomeAuthorizationFilter: AuthorizeAttribute
    {
            private readonly string[] _allowedRoles;
            public CustomeAuthorizationFilter(params string[] role)
            {
                this._allowedRoles = role;
            }
            protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                var authorize = false;
                var userId = Convert.ToString(httpContext.Session["userID"]);
                if (!string.IsNullOrEmpty(userId))
                {
                var user = (ClientView)httpContext.Session["user"];
                    foreach (var role in _allowedRoles)
                    {
                        if (role == user.Type.ToString()) return true;
                    }
                }
                return authorize;
            }
            protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                    { "controller", "Account" },
                    { "action", "unAuthorized " }
                    });
            }
        }
}