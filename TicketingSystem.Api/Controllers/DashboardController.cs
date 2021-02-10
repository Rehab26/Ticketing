using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TicketingSystem.Business.Managers;
using TicketingSystem.Common.Enums;
using TicketingSystem.Logging;

namespace TicketingSystem.Api.Controllers
{
    public class DashboardController : ApiController
    {

        DashboardManager manager = new DashboardManager();

        [HttpGet]
        [System.Web.Http.Route("api/Dashboard/CountAllEmployees")]
        public HttpResponseMessage CountAllEmployees()
        {
            try
            {
                var response = manager.GetAllEmployeesCount();

                if (response > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No employees found.");
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [System.Web.Http.Route("api/Dashboard/CountAllClients")]
        public HttpResponseMessage CountAllClients()
        {
            try
            {
                var response = manager.GetAllClientsCount();

                if (response > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, 0);
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [System.Web.Http.Route("api/Dashboard/CountAllTickets")]
        public HttpResponseMessage CountAllTickets(int id = 0)
        {
            try
            {
                var response = manager.GetAllTicketsCount(id);

                if (response > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, 0);
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [System.Web.Http.Route("api/Dashboard/CountTicketsByStatus")]
        public HttpResponseMessage CountTicketsByStatus(TicketStatus status, int id = 0)
        {
            try
            {
                var response = manager.GetTicketsByStatusCount(status, id);

                if (response > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, 0);
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
      
        [HttpGet]
        [System.Web.Http.Route("api/Dashboard/TopFiveEmployees")]
        public HttpResponseMessage GetTopFiveEmployees()
        {
            try
            {
                var response = manager.GetTopEmployees();

                if (response != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]

        [System.Web.Http.Route("api/Dashboard/CountTicketsByCountry")]
        public HttpResponseMessage CountTicketsByCountry(UserCountry country)
        {
            try
            {
                var response = manager.GetTicketsByUserCountryCount(country);

                if (response > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, 0);
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

    }
}
