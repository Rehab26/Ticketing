using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TicketingSystem.Business.Managers;
using TicketingSystem.Logging;

namespace TicketingSystem.Api.Controllers
{
    public class HealthCheckController : ApiController
    {
        HealthManager manager = new HealthManager();

        // Check database connection by counting the number of users in the system
        [HttpGet]
        [System.Web.Http.Route("api/HealthCheck/DBHealthCheck")]
        public HttpResponseMessage DBHealthCheck()
        {
            try
            {
                var response = manager.CountUsers();

                if (response > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Healthy Connection with Database");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Unhealthy Connection with Database!");
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // Check storage connection by counting the number of files/attachments in the system
        [HttpGet]
        [System.Web.Http.Route("api/HealthCheck/StorageHealthCheck")]
        public HttpResponseMessage StorageHealthCheck()
        {

            try
            {
                var response = manager.CountFiles();

                if (response > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Healthy Connection with Storage");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Unhealthy Connection with Storager OR No Files Exists in the Storage Yet!");
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            }

        }

    }
}
