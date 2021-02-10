using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace TicketingSystem.Api
{
    public class CustomAuthenticationFilter : AuthorizeAttribute, IAuthenticationFilter
    {
        // This filter is to make sure that the user who get the response is the one ask for information
        public bool AllowMultiple
        {

            get { return false; }

        }
        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            string authParamter = string.Empty;
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            string[] TokenAndUser = null;

            if (authorization == null)
            {
                context.ErrorResult = new authenticationFailureResult(reasonPhrase: "Missing Authorization header", request);
                return;
            }
            if (authorization.Scheme != "Bearer")
            {
                context.ErrorResult = new authenticationFailureResult(reasonPhrase: "Invalid Authorization Scheme", request);
                return;
            }

            TokenAndUser = authorization.Parameter.Split(':'); // params separator:

            string Token = TokenAndUser[0];
            string username = TokenAndUser[1];

            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new authenticationFailureResult(reasonPhrase: "Missing Token", request);
                return;
            }

            string ValidUserName = TokenManager.ValidateToken(Token);
            if (username != ValidUserName)
            {
                context.ErrorResult = new authenticationFailureResult(reasonPhrase: "Invalid Token for user", request);
                return;
            }

            context.Principal = TokenManager.GetPrincipal(Token);
        }

        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            var result = await context.Result.ExecuteAsync(cancellationToken);
            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                result.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue(scheme: "Basic", parameter: "realm=localhost"));
            }
            context.Result = new ResponseMessageResult(result);
        }
    }

    public class authenticationFailureResult : IHttpActionResult
    {
        public string ReasonPhrase;
        public HttpRequestMessage Request { get; set; }
        public authenticationFailureResult(string reasonPhrase, HttpRequestMessage request)
        {
            ReasonPhrase = reasonPhrase;
            Request = request;
        }
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        public HttpResponseMessage Execute()
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            responseMessage.RequestMessage = Request;
            responseMessage.ReasonPhrase = ReasonPhrase;
            return responseMessage;

        }
    }
}