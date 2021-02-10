using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TicketingSystem.Business.Managers;
using TicketingSystem.Common.Models.UserModels;
using TicketingSystem.Logging;

namespace TicketingSystem.Api.Controllers
{
   // [Log]
    public class AccountController : ApiController
    {

        AccountManager manager = new AccountManager();


        //POST
        [System.Web.Http.Route("api/Account/CreateEmployee")]
        public HttpResponseMessage CreateEmployee(ViewUser user)
        {
            try
            {
                var isPosted = manager.CreateEmployee(user);
                if (isPosted)
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
          
        }

        [HttpPost]
        [ActionName("ClientRegister")]
        public HttpResponseMessage CreateClient(ViewUser user)
        {

            try
            {
                var token = TokenManager.GenerateToken(user.PhoneNumber);
                var client = manager.CreateClient(user);
                client.Token = token;
                if (client != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, client);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

        }
        [HttpGet]
        public HttpResponseMessage UserCheck(string phoneNo, string Email)
        {
            try
            {
                var response = manager.isValid(phoneNo, Email);
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

        }
        [ActionName("CheckLoginType")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage EmployeeLoginTypeCheck(string phoneNo)
        {
            try
            {
                var response = manager.CheckLoginApproach(phoneNo);
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

        }

        [HttpPost]
        public HttpResponseMessage LoginEmployee(ViewUser user)
        {
            try
            {
                //added for token
                var token = TokenManager.GenerateToken(user.PhoneNumber); 
                user.Token = token; 
                var loggedUser = manager.LoginEmployee(user);
                if (loggedUser != null)
                return Request.CreateResponse(HttpStatusCode.OK,loggedUser);
                else
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [ActionName("ClientLogin")]
        public IHttpActionResult LoginClient(ClientLogin login)
        {
            try
            {
                //Generate token
                var token = TokenManager.GenerateToken(login.PhoneNumber); 
                login.Token = token; 
                return Ok(manager.LoginClient(login));
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return InternalServerError();
            }
        }

        [HttpPost]
        [ActionName("LoginManager")]
        public IHttpActionResult LoginManager(ViewUser login)
        {
            try
            {
                //Generate token
                var token = TokenManager.GenerateToken(login.PhoneNumber); 
                login.Token = token;
                var loggedUser = manager.LoginManager(login);
                if (loggedUser != null)
                    return Ok(loggedUser);
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return InternalServerError();
            }
        }


        // Verify email to change user passsword
        [HttpPost]
        public IHttpActionResult VerifyEmail(PasswordForget passwordModel)
        {
            try
            {
                var user = manager.IsUserExists(passwordModel);
                if (user != null)
                    return Ok(passwordModel);

                else
                    return InternalServerError();
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return InternalServerError();
            }

        }

        // Send email to change user passsword
        [HttpPost]
        public IHttpActionResult SendEmail(PasswordForget passwordModel)
        {
            try
            {
                var user = manager.SendEmail(passwordModel);

                if (user != null)
                    return Ok(passwordModel);

                else
                    return InternalServerError();
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return InternalServerError();
            }

        }

        // Set new passsword 
        [HttpPost]
        public IHttpActionResult NewPassword(PasswordReset passwordModel)
        {
            try
            {
                return Ok(manager.ResetPassword(passwordModel));
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return InternalServerError();
            }
        }
    }
}
