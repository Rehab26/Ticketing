using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;
using TicketingSystem.Logging;

namespace TicketingSystem.Api
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public async override Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            // Access Exception using context.Exception;  
            const string errorMessage = "An unexpected error occured";
            var response = context.Request.CreateResponse(HttpStatusCode.InternalServerError,
                new
                {
                    Message = errorMessage
                });
            GlobalVariable.log.Write(LogLevel.Error, context.Exception);
            response.Headers.Add("X-Error", errorMessage);
            context.Result = new ResponseMessageResult(response);
        }

    }
}