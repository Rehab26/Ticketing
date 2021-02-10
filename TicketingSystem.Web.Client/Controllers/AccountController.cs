using System;
using System.Web.Mvc;
using TicketingSystem.Web.Client.Models;
using TicketingSystem.Logging;
using TicketingSystem.RiCHService;
using TicketingSystem.Web.Client.Filters;
using System.IO;

namespace TicketingSystem.Web.Client.Controllers
{
    public class AccountController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Login()
        {
            
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginModel login)
        {

            if (ModelState.IsValid)
            {
                // Hash the password before send it
                //login.Password = login.Password.Hash();
                var httplCient = Connector.GetHttpClient();
                var stringContent = Connector.GetStringOfObject(login);
                //Exception here
                try
                {
                    var response = httplCient.PostAsync("Account/ClientLogin", stringContent).Result;
                    var httpResponseContent = response.Content.ReadAsStringAsync().Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var user = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<ClientView>(httpResponseContent);
                        if (user == null)
                        {
                            GlobalVariable.log.Info($"Wrong try login with {login.PhoneNumber} and {login.Password}");
                            ModelState.AddModelError("", "Invalid login attempt, try with correct phone number or password");
                            return View(login);
                        }
                        else
                        {
                            Session["userID"] = user.Id;
                            Session["user"] = user;
                            Session["TokenNumber"] = user.Token; //added for token
                            Session["UserName"] = user.PhoneNumber; //added for token

                            return RedirectToAction("profile", "Ticket");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Server error, please try later");
                    }

                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        if (ex.InnerException.InnerException != null)
                        {
                            GlobalVariable.log.Error(ex.InnerException.InnerException.Message, ex);
                        }
                    }
                    else GlobalVariable.log.Error(ex.Message, ex);
                    ModelState.AddModelError("", "Server error, please try later");
                    return View(login);
                }
            }
            return View(login);
        }
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View("Register");
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Authentication(AuthenticationModel Auth)
        {
            if (ModelState.IsValid)
            {
                if (Auth.InputCode == Session["OriginalCode"].ToString())
                {
                    try
                    {
                        var httpCient = Connector.GetHttpClient();
                        // for token
                        var client = (ClientSaveModel)Session["signUser"];
                        Session.Abandon();
                        Session.Clear();
                        var stringContent = Connector.GetStringOfObject(client);
                        var response = httpCient.PostAsync("Account/ClientRegister", stringContent).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            ViewData.Clear();
                            return RedirectToAction("Login", "Account");
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobalVariable.log.Error(ex.StackTrace, ex);
                        ModelState.AddModelError("", "Server error, please try later");
                        return View("Authentication");
                    }
                }
                ModelState.AddModelError("", "Wrong code");
                return View("Authentication", Auth);
            }
            return View("Authentication", Auth);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(ClientSaveModel client)
        {
            if (ModelState.IsValid)
            {
                try
                {               
                    // add new user of type client to the database
                    var httpClient = Connector.GetHttpClient();
                    var response = httpClient.GetAsync($"Account/UserCheck?PhoneNo={client.PhoneNumber}&Email={client.Email}").Result;
                    var httpResponseContent = response.Content.ReadAsStringAsync().Result;
                    var responseMessage = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<string>(httpResponseContent);
                    if (responseMessage == "Valid")
                    {
                        var RiCHSMS = new RiCHSMSManager();
                        var RichResponse = RiCHSMS.SendSmsSignUp(client.PhoneNumber);
                        if (RichResponse.IsSent)//Simulation with Rich by send clientAuth.OriginalCode to Rich api
                        {
                            Session["OriginalCode"] = RichResponse.OTPassword;
                            Session["signUser"] = client;
                            return View("Authentication");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", responseMessage);
                        return View();
                    }

                }
                catch (Exception e)
                {
                    if (e.InnerException != null)
                    {
                        if (e.InnerException.InnerException != null)
                        {
                            GlobalVariable.log.Error(e.InnerException.InnerException.Message);
                        }
                    }
                    else GlobalVariable.log.Error(e.Message);
                    ModelState.AddModelError("", "Something went wrong, please try later");
                    return View(client);
                }
            }
            return View(client);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ForgotPassword(ForgetPassword changePassword)
        {
            // Get link for reset password          
            changePassword.Link = GetLink();

            var httplCient = Connector.GetHttpClient();
            var stringContent = Connector.GetStringOfObject(changePassword);
            //Exception here
            try
            {
                var response = httplCient.PostAsync("Account/VerifyEmail", stringContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    var httplCient2 = Connector.GetHttpClient();
                    var stringContent2 = Connector.GetStringOfObject(changePassword);
                    // Send email with change pasword link
                    var response2 = httplCient2.PostAsync("Account/SendEmail", stringContent2).Result;
                    var httpResponseContent2 = response2.Content.ReadAsStringAsync().Result;

                    if (response2.IsSuccessStatusCode)
                    {
                        ViewBag.Message = "An email has been sent to you containing link to reset your password.";
                        return View();
                    }
                    return View(changePassword);

                }
                else
                {
                    ModelState.AddModelError("", "Email is not found! Enter correct Information");
                }

            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        Log.Error(ex.InnerException.InnerException.Message, ex);
                    }
                }
                else Log.Error(ex.Message, ex);
                ModelState.AddModelError("", "Server error, please try later");
                return View(changePassword);
            }

            return View(changePassword);
        }
       
        [AllowAnonymous]
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ChangePassword(ResetPassword newPassword)
        {
            if (newPassword.Password == newPassword.ConfirmPassword)
            {

                // Add info to the model
                newPassword.Link = Path.GetFileName(Request.Url.AbsolutePath);

                var httplCient = Connector.GetHttpClient();
                var stringContent = Connector.GetStringOfObject(newPassword);
                //Exception here
                try
                {
                    var response = httplCient.PostAsync("Account/NewPassword", stringContent).Result;
                    var httpResponseContent = response.Content.ReadAsStringAsync().Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var user = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<ClientView>(httpResponseContent);
                        if (user == null)
                        {
                            Log.Info($"Wrong try changing to new password");
                            ModelState.AddModelError("", "Invalid attempt! Make sure you enter correct information");
                            return View(newPassword);
                        }
                        else
                        {
                            return RedirectToAction("Login", "Account");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Server error, please try later");
                    }

                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        if (ex.InnerException.InnerException != null)
                        {
                            Log.Error(ex.InnerException.InnerException.Message, ex);
                        }
                    }
                    else Log.Error(ex.Message, ex);
                    ModelState.AddModelError("", "Server error, please try later");
                    return View(newPassword);
                }

                return RedirectToAction("Login", "Account");
            }
            else
                ModelState.AddModelError("", "Password and Confirm Password does not match! Try again ");

            return View(newPassword);

        }
        public ActionResult Logout()
        {

            Session.Abandon();
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        public ActionResult UnAuthorized()
        {
            return View("_NotFound");
        }

        [NonAction]
        public string GetLink()
        {
            // Create unique link for each reset password request
            string resetCode = Guid.NewGuid().ToString();
            var verifyUrl = "/Account/ChangePassword/" + resetCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            return link;
        }
    }
}