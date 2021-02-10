
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using System.Web.Mvc;
using TicketingSystem.Logging;

namespace TicketingSystem.Api
{
    public class LogAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        public LogAttribute()
        {

        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                var controller = actionContext.RequestContext.RouteData.Values["Controller"];
                var action = actionContext.RequestContext.RouteData.Values["Action"];
                //var request = filterContext.ActionParameters.Values;
                var request = new JsonResult();
                request.Data = actionContext.ActionArguments.Values;
                string message = "";
                if (request.Data is IEnumerable<object>)
                {
                    var requestData = (IEnumerable<object>)request.Data;
                    if (!requestData.Any())
                    {
                        message = string.Format("controller:{0} action:{1} request:{2}", controller, action, GetStringOfObject(request).ReadAsStringAsync().Result);

                    }
                    foreach (var data in requestData)
                    {
                        if (data is HttpPostedFileBase[] || data is HttpPostedFileBase)
                        {
                            continue;
                        }
                        message = String.Format("controller:{0} action:{1} request:{2}", controller, action, GetStringOfObject(data).ReadAsStringAsync().Result);

                    }
                }
                GlobalVariable.log.Write(LogLevel.Debug, message);
                base.OnActionExecuting(actionContext);
            }
            catch (Exception ex) {
                GlobalVariable.log.Write(LogLevel.Error,ex);
            }
            
            }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                var response = actionExecutedContext.Response.Content;

                    var message = String.Format("Response:{0}", GetStringOfObject(response).ReadAsStringAsync().Result);
                    GlobalVariable.log.Write(LogLevel.Debug, message);
         
            }
            catch (Exception ex) {
                GlobalVariable.log.Write(LogLevel.Error, ex);
            }
        }

        public StringContent GetStringOfObject(Object obj)
        {
            try
            {
                var stringContent = new StringContent(
                    JsonConvert.SerializeObject(obj),
                    Encoding.UTF8,
                    "application/json");

                return stringContent;
            }
            catch (Exception ex) {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return null;
            }
        }
    }
}