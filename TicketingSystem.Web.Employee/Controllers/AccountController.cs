using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using NAudio.Wave;
using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Web.Mvc;
using TicketingSystem.Common.Enums;
using TicketingSystem.Logging;
using TicketingSystem.RiCHService;
using TicketingSystem.Web.Employee.Models;

namespace TicketingSystem.Web.Employee.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        public string inFilePath = "";
        public string outFilePath = "";
        public ActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public ActionResult AddNewUser(UserRegistrationModel user)
        {
            try
            {
                if (Session["filePath"] == null)
                {
                    ModelState.AddModelError("", "Please record your voice or take picture");
                    return View("Register", user);
                }

                if (Session["filePath"] != null && IsAudio(Session["filePath"].ToString()) && GetAudioDuration(Session["filePath"].ToString()).Value.TotalSeconds <= 5)
                {
                    ModelState.AddModelError("", "The duration of the audio recording should be more than 5 seconds");
                    return View("Register", user);
                }


                if (ModelState.IsValid)
                {

                    user.File = Session["filePath"].ToString();
                    user.Id = Convert.ToInt32(Session["UserID"]);

                    var fileExtention = user.File.Split('.');
                    if (IsImage(Session["filePath"].ToString()))
                    {
                        user.LoginType = LoginApproach.Face;
                    }
                    else if (IsAudio(Session["filePath"].ToString()))
                    {
                        user.LoginType = LoginApproach.Voice;
                    }
                    else
                    {
                        ModelState.AddModelError("", "The Email address or the phone number already exists!");
                    }
                    var httpClient = Connector.GetHttpClient();
                    var response = httpClient.GetAsync($"Account/UserCheck?PhoneNo={user.PhoneNumber}&Email={user.Email}").Result;
                    var httpResponseContent = response.Content.ReadAsStringAsync().Result;
                    var responseMessage = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<string>(httpResponseContent);
                    if (responseMessage == "Valid")
                    {
                        var RiCHSMS = new RiCHSMSManager();
                        var RichResponse = RiCHSMS.SendSmsSignUp(user.PhoneNumber);
                        if (RichResponse.IsSent) //Simulation with Rich by send clientAuth.OriginalCode to Rich api
                        {
                            Session["OriginalCode"] = RichResponse.OTPassword;
                            Session["signUser"] = user;
                            return View("Authentication");
                        } 
                        ModelState.AddModelError("", "Something went wrong, please try later");
                        return View("Register", user);
                    }
                    else
                    {
                        ModelState.AddModelError("", responseMessage);
                        return View("Register" , user);
                    }

                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                ModelState.AddModelError("" , "There is an error, please try later");
            }

            return View("Register", user);
        }
        public ActionResult Authentication(AuthenticationModel authentication)
        {
            if (ModelState.IsValid)
            {
                if (authentication.InputCode == Session["OriginalCode"].ToString())
                {
                    try
                    {
                        var httpCient = Connector.GetHttpClient();
                        // for token
                        var user = (UserRegistrationModel)Session["signUser"];
                        var response = httpCient.PostAsJsonAsync<UserRegistrationModel>("Account/CreateEmployee", user).Result;
                        var httpResponseContent = response.Content.ReadAsStringAsync().Result;
                        if (response.IsSuccessStatusCode)
                        {
                            Session.Abandon();
                            Session.Clear();
                            ViewData.Clear();
                            return RedirectToAction("UserLogin", "Account");
                        }
                    }
                    catch (Exception ex)
                    {
                        GlobalVariable.log.Error(ex.StackTrace, ex);
                        ModelState.AddModelError("", "Something went wrong, please try later");
                        return View("Authentication");
                    }
                }
                ModelState.AddModelError("", "Wrong code");
                return View("Authentication", authentication);
            }
            return View("Authentication", authentication);
        }
        public ActionResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserLoginModel loginModel)
        {
            try
            {

                if (Session["filePath"] == null && loginModel.Password == null)
                {
                    ModelState.AddModelError("", "Please enter your credentials");
                    return View("UserLogin", loginModel);
                }

                if (ModelState.IsValid)
                {
                    //Check for the login type of the user
                    var httpClient = Connector.GetHttpClient();
                    var LoginResponse = httpClient.GetAsync($"Account/CheckLoginType?PhoneNo={loginModel.PhoneNumber}").Result;
                    var httpResponseContent = LoginResponse.Content.ReadAsStringAsync().Result;
                    var responseMessage = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<LoginApproach?>(httpResponseContent);
                    if (responseMessage == null)
                    {
                        ModelState.AddModelError("", "your number doesn't exist");
                        return View("UserLogin", loginModel);
                    }
                    loginModel.LoginType = (LoginApproach)responseMessage;
                    if (Session["filePath"] != null)
                    {
                        loginModel.File = Session["filePath"].ToString();

                        if (IsImage(Session["filePath"].ToString()) && responseMessage == LoginApproach.Voice)
                        {
                            ModelState.AddModelError("", "your login approach saved as VoiceId please record your voice");
                            return View("UserLogin", loginModel);
                        }
                        if (IsAudio(Session["filePath"].ToString()) && responseMessage == LoginApproach.Face)
                        {
                            ModelState.AddModelError("", "your login approach saved as FaceId please capture your face");
                            return View("UserLogin", loginModel);
                        }
                        if ((loginModel.Password == null) && responseMessage == LoginApproach.Password)
                        {
                            ModelState.AddModelError("", "your login approach saved as Password please enter your password");
                            return View("UserLogin", loginModel);
                        }
                        if (Session["filePath"] != null && IsAudio(Session["filePath"].ToString()))
                        {
                            var path = Session["filePath"].ToString();
                            var duration = GetAudioDuration(path);
                            if (duration == null)
                            {
                                ModelState.AddModelError("", "Please record your voice without any noise around you");
                                return View("UserLogin", loginModel);
                            }
                            if (IsAudio(path) && duration.Value.TotalSeconds <= 5 && loginModel.Password == null)
                            {
                                ModelState.AddModelError("", "The duration of the audio recording should be more than 5 seconds");
                                return View("UserLogin", loginModel);
                            }
                        }
                    }
                    var client = Connector.GetHttpClient();
                    HttpResponseMessage response = null;
                    if (responseMessage == LoginApproach.Password && loginModel.Password != null)
                    {
                        response = client.PostAsJsonAsync<UserLoginModel>("Account/LoginManager", loginModel).Result;
                    }
                    else if (loginModel.File != null && (loginModel.LoginType == LoginApproach.Face || loginModel.LoginType == LoginApproach.Voice))
                    {
                        response = client.PostAsJsonAsync<UserLoginModel>("Account/LoginEmployee", loginModel).Result;
                    }
                    if (response!= null && response.IsSuccessStatusCode)
                    {
                        var user = response.Content.ReadAsAsync<UserLoginModel>().Result;
                        if (user != null)
                        {
                            Session["User"] = user;
                            Session["userId"] = user.Id;
                            Session["FirstName"] = user.FirstName;
                            Session["UserType"] = user.Type;
                            Session["TokenNumber"] = user.Token; 
                            Session["UserName"] = user.PhoneNumber; 
                                                                     
                            // if the user is manager
                            if (user.Type == UserType.Manager)
                            {
                                return RedirectToAction("Dashboard", "Dashboard");
                            }
                            // if the user is an employee
                            if (user.Type == UserType.Employee)
                            {
                                return RedirectToAction("DisplayEmployeeTickets", "Ticket");
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Check the mobile number or your credentials");
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                ModelState.AddModelError("", "Something went wrong, please try again later");
            }
            return View("UserLogin", loginModel);
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction("UserLogin", "Account");
        }
        public ActionResult UnAuthorized()
        {
            return View("_NotFound");
        }

        [HttpPost]
        public void SaveAudio()
        {
            try
            {
                var yourSharedStoragePath = ConfigurationManager.AppSettings["UserAttachments"];
                var blobObject = Request.Files["data"];
                if (blobObject != null)
                {
                    blobObject.InputStream.Seek(0, SeekOrigin.Begin);
                    inFilePath = $"{yourSharedStoragePath}{Guid.NewGuid().ToString("N")}.mp3";
                    outFilePath = $"{yourSharedStoragePath}{Guid.NewGuid().ToString("N")}.wav";
                    blobObject.SaveAs(inFilePath);
                    ConvertMp3ToWav(inFilePath, outFilePath);
                    Session["filePath"] = outFilePath;
                }
                

            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
            }
        }

        private static void ConvertMp3ToWav(string _inPath_, string _outPath_)
        {
            try
            {
                using (MediaFoundationReader mp3 = new MediaFoundationReader(_inPath_))
                {
                    using (WaveStream pcm = WaveFormatConversionStream.CreatePcmStream(mp3))
                    {

                        WaveFileWriter.CreateWaveFile(_outPath_, pcm);

                    }
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
            }
        }

        private static TimeSpan? GetAudioDuration(string filePath)
        {
            try
            {
                using (var shell = ShellObject.FromParsingName(filePath))
                {
                    IShellProperty prop = shell.Properties.System.Media.Duration;
                    var t = (ulong) prop.ValueAsObject;
                    return TimeSpan.FromTicks((long) t);
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
            }
            return null;
        }

        protected bool IsAudio(string path)
        {
            var fileExtention = path.Split('.');
            if (fileExtention[1] == "mp3" || fileExtention[1] == "wav")
            {
                return true;
            }
            return false;
        }

        protected bool IsImage(string path)
        {
            var fileExtention = path.Split('.');
            if (fileExtention[1] == "jpg" || fileExtention[1] == "png")
            {
                return true;
            }
            return false;
        }
    }
}
