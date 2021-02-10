using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TicketingSystem.Business.Managers;
using TicketingSystem.Common.Models.Paging;
using TicketingSystem.Common.Models.TicketModels;
using TicketingSystem.Logging;

namespace TicketingSystem.Api.Controllers
{

    [Log]
    public class TicketController : ApiController
    {
        // logging 
        static ILogger _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        TicketManager manager = new TicketManager();

        //GET
        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage All(int id, PagingModel paging)
        {
            try
            {
                var response = manager.GetAllTickets(id, paging);

                if (response != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, "No tickets yet!");
                }

            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return Request.CreateResponse(HttpStatusCode.NoContent, "No tickets yet!");
            }

        }
        [HttpPost]
        [CustomAuthenticationFilter]
        [System.Web.Http.Route("api/Ticket/GetTickets")]
        public HttpResponseMessage GetTickets(int id, PagingModel paging)
        {
            try
            {
                var response = manager.GetAllTickets(id, paging);

                if (response != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, "No tickets yet!");
                }

            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return Request.CreateResponse(HttpStatusCode.NoContent, "No tickets yet!");
            }

        }

        [HttpGet]
        [CustomAuthenticationFilter]
        public HttpResponseMessage Get(int id, int userId)
        {
            try
            {
                var ticket = manager.GetTicket(id, userId);

                if (ticket != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, ticket);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Ticket not found for ID: " + id);
                }

            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return Request.CreateResponse(HttpStatusCode.NotFound, "Ticket not found for ID: " + id);
            }

        }


        [HttpGet]
        [CustomAuthenticationFilter]
        public HttpResponseMessage GetComments(int id)
        {
            try
            {
                var comments = manager.ViewReplys(id);

                if (comments != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, comments);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Ticket not found for ID: " + id);
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return Request.CreateResponse(HttpStatusCode.NotFound, "Ticket not found for ID: " + id);
            }

        }

        [HttpGet]
        [CustomAuthenticationFilter]
        [ActionName("getClientTickets")]
        public HttpResponseMessage GetClientTickets(int id)
        {
            try
            {
                var response = manager.GetClientTickets(id);

                if (response != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No tickets found for client:" + id);
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return Request.CreateResponse(HttpStatusCode.NotFound, "Ticket not found for ID: " + id);
            }
        }

        [HttpGet]
        [CustomAuthenticationFilter]
        [ActionName("getTicketAttachments")]
        public HttpResponseMessage GetTicketAttachments(int id)
        {

            try
            {
                var response = manager.GetTicketAttachment(id);

                if (response != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, response);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, response);
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return Request.CreateResponse(HttpStatusCode.NotFound, "Ticket not found for ID: " + id);
            }
        }


        [HttpGet]
        [CustomAuthenticationFilter]
        [System.Web.Http.Route("api/Ticket/ViewTicket")]
        public HttpResponseMessage ViewTicket(int id)
        {

            try
            {
                var ticket = manager.ViewTicket(id);

                if (ticket)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, ticket);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Ticket not found for ID: " + id);
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return Request.CreateResponse(HttpStatusCode.NotFound, "Ticket not found for ID: " + id);
            }

        }

        [HttpGet]
        [CustomAuthenticationFilter]
        public HttpResponseMessage GetTiecktByID(int id)
        {

            try
            {
                var ticket = manager.GetSpecificTieckt(id);

                if (ticket != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, ticket);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Ticket not found with ID: " + id);
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return Request.CreateResponse(HttpStatusCode.NotFound, "Ticket not found for ID: " + id);
            }

        }

        //POST
        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage PostTicket(TicketSave ticket)
        {
            try
            {
                return Request.CreateResponse(manager.AddTicket(ticket));
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Ticket cannot be created!");
            }

        }

        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage PostReply(ReplyModel reply)
        {

            try
            {
                var isAdded = manager.AddReply(reply);

                if (isAdded)
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Reply cannot be added!");
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return Request.CreateResponse(HttpStatusCode.NotFound, "Reply not found for Ticket ID: " + reply.TicketId);
            }
        }

        //PUT
        [HttpPut]
        [CustomAuthenticationFilter]
        [System.Web.Http.Route("api/Ticket/EditTicketStatus")]
        public HttpResponseMessage EditTicketStatus(TicketSave ticket)
        {

            try
            {
                var editStatus = manager.EditTicketStatus(ticket);

                if (editStatus)
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Ticket status is not updated!");
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return Request.CreateResponse(HttpStatusCode.NotFound, "Can not update Ticket with ID: " + ticket.Id);
            }
        }

        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage EditTicket(TicketSave ticket)
        {

            try
            {
                var Editerticket = manager.EditTicket(ticket);
                if (Editerticket != 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, Editerticket);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Ticket is not updated!");
                }

            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return Request.CreateResponse(HttpStatusCode.NotFound, "Can not update Ticket with ID: " + ticket.Id);
            }
        }

        [HttpPut]
        [CustomAuthenticationFilter]
        [System.Web.Http.Route("api/Ticket/AssignTicket")]
        public HttpResponseMessage PutAssignTicket(TicketSave ticket)
        {

            try
            {
                var editStatus = manager.AssignTicketToEmployee(ticket);

                if (editStatus)
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Ticket is not updated!");
                }

            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return Request.CreateResponse(HttpStatusCode.NotFound, "Can not update Ticket with ID: " + ticket.Id);
            }
        }

    }
}
