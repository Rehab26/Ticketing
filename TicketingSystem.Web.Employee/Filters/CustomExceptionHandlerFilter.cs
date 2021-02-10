using System.Web.Mvc;
using TicketingSystem.Logging;

namespace TicketingSystem.Web.Employee.Filters
{
    public class CustomExceptionHandlerFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {

                //Log th exeption
                GlobalVariable.log.Write(LogLevel.Error, filterContext.Exception);

                filterContext.ExceptionHandled = true;
                filterContext.Result = new ViewResult()
                {
                    ViewName = "Error"
                };
            }
        }
    }
}