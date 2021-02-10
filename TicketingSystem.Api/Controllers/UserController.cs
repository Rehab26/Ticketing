using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TicketingSystem.Business.Managers;
using TicketingSystem.Common.Models.Paging;
using TicketingSystem.Common.Models.UserModels;
using TicketingSystem.Logging;


namespace TicketingSystem.Api.Controllers
{
    [Log]
    public class UserController : ApiController
    {
        // logging 
        static ILogger _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        UserManager manager = new UserManager();

        //GET
        [System.Web.Http.Route("api/User/GetAllEmployees")]
        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage GetAllEmployees(PagingModel paging)
        {
            try
            {
                var employees = manager.GetAllEmployees(paging);

                if (employees == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, employees);
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [System.Web.Http.Route("api/User/GetAllClients")]
        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage GetAllClients(PagingModel paging)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, manager.GetAllClients(paging));
                //return Request.CreateResponse(HttpStatusCode.OK, manager.GetClientsWithTickets(paging));
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [ActionName("get")]
        [HttpGet]
        [CustomAuthenticationFilter]
        public IHttpActionResult GetSpecificUser(int id)
        {
            try
            {
                return Ok(manager.GetUser(id));
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return InternalServerError();
            }
        }

        [ActionName("getClient")]
        [HttpGet]
        [CustomAuthenticationFilter]
        public IHttpActionResult GetSpecificClient(int id)
        {
            try
            {
                return Ok(manager.GetClient(id));
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return InternalServerError();
            }
        }
        [HttpGet]
        [CustomAuthenticationFilter]
        public IHttpActionResult GetAllEmployee()
        {
            try
            {
                return Ok(manager.GetAllEmployees());
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return InternalServerError();
            }
        }

        //PUT
        [HttpGet]
        [CustomAuthenticationFilter]
        [System.Web.Http.Route("api/User/UserActivation")]
        public HttpResponseMessage ChangeUserStatus(int id)
        {
            try
            {
                var activateSatus = manager.UserActivatation(id);

                if (activateSatus == false)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [CustomAuthenticationFilter]
        [System.Web.Http.Route("api/User/EditUser")]
        public HttpResponseMessage PutUser(int id, EditUserView userDetails)
        {
            try
            {
                var updateStatus = manager.EditUser(id, userDetails);

                if (updateStatus == false)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
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
