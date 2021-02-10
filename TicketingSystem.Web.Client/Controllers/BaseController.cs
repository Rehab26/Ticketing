using System.Web.Mvc;
using WebConnector;
using TicketingSystem.Web.Client.Models;
using TicketingSystem.Web.Client.Helper;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Web.Mvc.Filters;
using System.Net.Http.Headers;
using System.Net.Http;
using TicketingSystem.Common.Enums;

namespace TicketingSystem.Web.Client.Controllers
{
    public class BaseController : Controller
    {
        public ConnectManager Connector = new ConnectManager();
        public static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected int GetUserID()
        {
            return (int)Session["userId"];
        }
        protected HttpClient GetHttpClientWithSession()
        {
            var client = Connector.GetHttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                parameter: Session["TokenNumber"].ToString() + ":" + Session["UserName"]);
            return client;
        }
        protected ClientView GetClient()
        {
            var userDict = Get($"user/getClient/{GetUserID()}");
            return HelperMethod.GetObject<ClientView>(userDict);
        }
        protected string GetAttachment(int id, string url)
        {
            //url will be something like this -> Ticket/get || user/get
            var client = GetHttpClientWithSession();
            var response = client.GetAsync($"{url}/{id}").Result;
            return response.Content.ReadAsStringAsync().Result;
        }
        protected TicketAttachments GetTicketAttachment(int id)
        {
            var httpResponseContent = GetAttachment(id, "Ticket/getTicketAttachments");
            return new JavaScriptSerializer().Deserialize<TicketAttachments>(httpResponseContent);
        }
        protected Dictionary<string, object> Get(string url)
        {
            //url will be something like this -> Ticket/get || user/get
            var client = GetHttpClientWithSession();
            var response = client.GetAsync($"{url}").Result;
            if (response.IsSuccessStatusCode)
            {
                var httpResponseContent = response.Content.ReadAsStringAsync().Result;
                return new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(httpResponseContent);
            }
            return null;

        }
        protected TicketView GetTicket(int id)
        {
            var TicketDict = Get($"ticket/get?id={id}&userId={GetUserID()}");
            if (TicketDict == null) return null;
            var ticket = HelperMethod.GetObject<TicketView>(TicketDict);
            return ticket;
        }

        protected List<ReplyModel> GetComments(int ticketId)
        {
            var client = GetHttpClientWithSession();
            var httpResponse = client.GetAsync($"Ticket/getComments/{ticketId}").Result;
            var httpResponseContent = httpResponse.Content.ReadAsStringAsync().Result;
            var comments = new JavaScriptSerializer().Deserialize<List<ReplyModel>>(httpResponseContent);
            return comments;
        }
        //Method to get ticket info with all its replys and files
        protected TicketView GetTicketDetails(int id)
        {
            var ticket = GetTicket(id);
            if (ticket != null)
            {
                var attach = GetTicketAttachment(id);
                ticket.Attachments = attach.Attachments;
                var comments = GetComments(id);
                ticket.Comments = comments.ToArray();
                return ticket;
            }
            return null;

        }
        protected string GetTotal(string url)
        {
            var client = Connector.GetHttpClient();
            HttpResponseMessage result = client.GetAsync(url).Result;
            var content = result.Content.ReadAsStringAsync().Result;
            return content;
        }
        protected string GetTotal(TicketStatus status)
        {
            var client = Connector.GetHttpClient();
            HttpResponseMessage result = client.GetAsync($"Dashboard/CountTicketsByStatus?status={status}&id={GetUserID()}").Result;
            var content = result.Content.ReadAsStringAsync().Result;
            return content;
        }
        protected override void OnAuthentication(AuthenticationContext filterContext)
        {
            if (filterContext.Controller.GetType().Name == "HomeController" && filterContext.ActionDescriptor.ActionName == "index")
            {
                return;
            }
            if (filterContext.Controller.GetType().Name == "AccountController" && filterContext.ActionDescriptor.ActionName == "Login" || (filterContext.Controller.GetType().Name == "AccountController" && filterContext.ActionDescriptor.ActionName == "Register"))
            {
                if (Session.Keys.Count == 0)
                {
                    return;
                }

                filterContext.Result = new RedirectResult("~/Ticket/Profile");
            }
            bool checkForAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
            if (checkForAuthorization)
            {
                base.OnAuthentication(filterContext);
                return;
            }
            if (Session.Keys.Count == 0 && Session["userID"] == null)
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
                return;

            }
            base.OnAuthentication(filterContext);
        }
        protected override void HandleUnknownAction(string actionName)
        {
            ViewBag.actionName = actionName;
            View("_NotFound").ExecuteResult(this.ControllerContext);
        }

    }
}