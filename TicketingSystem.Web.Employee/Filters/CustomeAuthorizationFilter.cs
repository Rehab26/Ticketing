using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TicketingSystem.Web.Employee.Models;

namespace TicketingSystem.Web.Employee.Filters
{
    public class CustomeAuthorizationFilter : AuthorizeAttribute
    {
        private readonly string[] _allowedRoles;
        public CustomeAuthorizationFilter(params string[] role)
        {
            this._allowedRoles = role;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authorize = false;
            var user = Convert.ToString(httpContext.Session["UserName"]);
            if (!string.IsNullOrEmpty(user))
            {
                var userType = Convert.ToString(httpContext.Session["UserType"]);
                foreach (var role in _allowedRoles)
                {
                    if (role == userType) return true;
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
                    { "action", "UnAuthorized" }
                });
        }
    }
}