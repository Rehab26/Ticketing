using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Script.Serialization;
using TicketingSystem.Common.Enums;
using TicketingSystem.Logging;
using TicketingSystem.Web.Employee.Helpers;
using WebConnector;
using TicketingSystem.Web.Employee.Models;

namespace TicketingSystem.Web.Employee.Controllers
{
    public class BaseController : Controller
    {
        public ConnectManager Connector = new ConnectManager();
        protected string GetTotal(string url)
        {
            try
            {
                var client = Connector.GetHttpClient();
                HttpResponseMessage result = client.GetAsync(url).Result;
                var content = result.Content.ReadAsStringAsync().Result;
                return content;
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return null;
            }
        }
        protected string GetTotal(TicketStatus status, int id = 0)
        {
            try
            {
                var client = Connector.GetHttpClient();
                HttpResponseMessage result = client.GetAsync($"Dashboard/CountTicketsByStatus?status={status}&id={id}").Result;
                var content = result.Content.ReadAsStringAsync().Result;
                return content;
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return null;
            }
        }
        protected string GetTotal(UserCountry country)
        {
            try
            {
                var client = Connector.GetHttpClient();
                HttpResponseMessage result = client.GetAsync("Dashboard/CountTicketsByCountry?country=" + country).Result;
                var content = result.Content.ReadAsStringAsync().Result;
                return content;
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return null;
            }
        }
        protected int GetUserID()
        {
            try
            {
                return (int)Session["userId"];
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return -1;
            }
        }
        protected HttpClient GetHttpClientWithSession()
        {
            try
            {
                var client = Connector.GetHttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: Session["TokenNumber"].ToString() + ":" + Session["UserName"]);
                return client;
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return null;
            }
        }
        protected ClientView GetClient()
        {
            try
            {
                var userDict = Get($"user/getClient/{GetUserID()}");
                return HelperMethod.GetObject<ClientView>(userDict);
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return null;
            }
        }
        protected string GetAttachment(int id, string url)
        {
            try
            {
                //url will be something like this -> Ticket/get || user/get
                var client = GetHttpClientWithSession();
                var response = client.GetAsync($"{url}/{id}").Result;
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return null;
            }
        }
        protected TicketAttachments GetTicketAttachment(int id)
        {
            try
            {
                var httpResponseContent = GetAttachment(id, "Ticket/getTicketAttachments");
                return new JavaScriptSerializer().Deserialize<TicketAttachments>(httpResponseContent);
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return null;
            }
        }
        protected Dictionary<string, object> Get(string url)
        {
            try
            {    
                var client = GetHttpClientWithSession();
                var response = client.GetAsync($"{url}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var httpResponseContent = response.Content.ReadAsStringAsync().Result;
                    return new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(httpResponseContent);
                }
                return null;
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return null;
            }

        }

        protected TicketView GetTicket(int id)
        {
            try
            {
                var TicketDict = Get($"ticket/get?id={id}&userId={GetUserID()}");
                if (TicketDict == null) return null;
                var ticket = HelperMethod.GetObject<TicketView>(TicketDict);
                return ticket;
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return null;
            }
        }
        protected List<ReplyModel> GetComments(int ticketId)
        {
            try
            {
                var client = GetHttpClientWithSession();
                var httpResponse = client.GetAsync($"Ticket/getComments/{ticketId}").Result;
                var httpResponseContent = httpResponse.Content.ReadAsStringAsync().Result;
                var comments = new JavaScriptSerializer().Deserialize<List<ReplyModel>>(httpResponseContent);
                return comments;
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return null;
            }
        }

        //Method to get ticket info with all its replys and files
        protected TicketView GetTicketDetails(int id)
        {
            try
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
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return null;
            }
        }


        protected override void OnAuthentication(AuthenticationContext filterContext)
        {
            try
            {
                bool checkForAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
                if (checkForAuthorization)
                {
                    base.OnAuthentication(filterContext);
                    return;
                }
                if (Session.Keys.Count == 0 || Session["userID"] == null)
                {
                    filterContext.Result = new RedirectResult("~/Account/UserLogin");
                    return;

                }
                base.OnAuthentication(filterContext);
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
            }
        }
        protected override void HandleUnknownAction(string actionName)
        {
            ViewBag.actionName = actionName;
            View("_NotFound").ExecuteResult(this.ControllerContext);
        }
    }
}