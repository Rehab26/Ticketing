using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TicketingSystem.Logging;

namespace TicketingSystem.Web.Employee.Filters
{
        public class LogAttribute : System.Web.Mvc.ActionFilterAttribute
        {
            public LogAttribute()
            {

            }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            try
            {
                var controller = filterContext.RequestContext.RouteData.Values["Controller"];
                var action = filterContext.RequestContext.RouteData.Values["Action"];
                //var request = filterContext.ActionParameters.Values;
                var request = new JsonResult();
                request.Data = filterContext.ActionParameters.Values;
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
                base.OnActionExecuting(filterContext);
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
            }
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
                var controller = filterContext.RequestContext.RouteData.Values["Controller"];
                var action = filterContext.RequestContext.RouteData.Values["Action"];
                var result = new JsonResult();
                result.Data = filterContext.Result;
                string message = "";
                if (result.Data is IEnumerable<object>)
                {
                    var resultData = (IEnumerable<object>)result.Data;
                    if (!resultData.Any())
                    {
                        message = string.Format("controller:{0} action:{1} request:{2}", controller, action, GetStringOfObject(result).ReadAsStringAsync().Result);

                    }
                    foreach (var data in resultData)
                    {
                        if (data is HttpPostedFileBase[] || data is HttpPostedFileBase)
                        {
                            continue;
                        }
                        message = String.Format("controller:{0} action:{1} request:{2}", controller, action, GetStringOfObject(result).ReadAsStringAsync().Result);

                    }
                }
                if (message == "") message = String.Format("controller:{0} action:{1} request:{2}", controller, action, GetStringOfObject(result.Data).ReadAsStringAsync().Result);

                GlobalVariable.log.Write(LogLevel.Debug, message);
            }

            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
            }
            base.OnActionExecuted(filterContext);
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
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return null;
            }
        }

    }
}