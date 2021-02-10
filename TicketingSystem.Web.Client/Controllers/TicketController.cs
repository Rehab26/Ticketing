using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TicketingSystem.Common.Enums;
using TicketingSystem.Common.Models.Paging;
using TicketingSystem.Logging;
using TicketingSystem.Web.Client.Filters;
using TicketingSystem.Web.Client.Models;

namespace TicketingSystem.Web.Client.Controllers
{
    [CustomeAuthorizationFilter("Client")]
    [Log]
    public class TicketController : BaseController
    {

        public ActionResult Info()
        {
            var client = (ClientView)Session["user"];
            return View(client);
        }
        public ActionResult Edit(int id)
        {
            try
            {
                //Authentication
                var client = Connector.GetHttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                parameter: Session["TokenNumber"].ToString() + ":" + Session["UserName"]);

                var ticket = GetTicket(id);
                if (ticket == null) return View("_NotFound");
                var ticketSave = new TicketSave
                {
                    Priority = ticket.Priority,
                    Category = ticket.Category,
                    Id = ticket.Id,
                    ClientId = GetUserID(),
                    Title = ticket.Title,
                    Description = ticket.Description
                };
                return View("Edit", ticketSave);
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex.Message);
                return HttpNotFound();
            }
        }

        [HttpPost]
        public ActionResult AddComment(ReplyModel reply , HttpPostedFileBase File)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var client = GetHttpClientWithSession();
                    if (File != null)
                    {
                        var file = File;
                        if (!(file.ContentType.Contains("image")))
                        {
                            ModelState.AddModelError("", "Please select only image files");
                            return Json(new { error = true, responseText = "Please select only image files" },
                            JsonRequestBehavior.AllowGet);
                        }
                        var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                        //To-do check if the folder exist, if not create it 
                        var uploadPath = ConfigurationManager.AppSettings["UserAttachments"];
                        var fileExtension = Path.GetExtension(file.FileName);
                        fileName = DateTime.Now.ToString("yyyyMMdd") + "-" + fileName.Trim() + fileExtension;
                        reply.Attachment = uploadPath + fileName;
                        var path = reply.Attachment;
                        GlobalVariable.log.Info($"new file of type {fileExtension} added to the server folder by {GetUserID()}");
                        //Save file to server folder 

                        file.SaveAs(path);
                    }
                    reply.UserId = GetUserID();
                    var stringContent = Connector.GetStringOfObject(reply);
                    //TODO
                    var response = client.PostAsync("Ticket/PostReply", stringContent).Result;
                    if (response.IsSuccessStatusCode)
                        return Json(new { success = true, message = "Comment added." },
                    JsonRequestBehavior.AllowGet);
                    else return Json(new { success = false, message = "Something Went Wrong" },
                    JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    GlobalVariable.log.Write(LogLevel.Error, ex);
                    ModelState.AddModelError("", "Something went wrong on server, please try later");
                }
            }
            return Json(new { error = true, responseText = "Something went wrong." },
                    JsonRequestBehavior.AllowGet);
        }

        // View client profile with last 5 created tickets
        public ActionResult Profile()
        {
            try
            {
                //Authentication
                var client = Connector.GetHttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                parameter: Session["TokenNumber"].ToString() + ":" + Session["UserName"]);
                client = GetHttpClientWithSession();

                //handle if this is null == user does not have access 
                var httpResponse = client.GetAsync($"Ticket/getClientTickets/{GetUserID()}").Result;
                var httpResponseContent = httpResponse.Content.ReadAsStringAsync().Result;
                var tickets = new JavaScriptSerializer().Deserialize<IEnumerable<TicketSave>>(httpResponseContent);
                var clientTicket = new ClientTicketView()
                {
                    Tickets = tickets.ToList(),
                    Client = GetClient()
                };
                if (tickets.Count() == 0)
                {
                    ViewBag.Message = "You don't have any ticket yet";
                    return View("Profile", clientTicket);
                }
                return View("Profile", clientTicket);
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
            }
            return HttpNotFound();

        }
        // Get ticket information
        public ActionResult Ticket(int id, int? Title)
        {
            //Authentication
            var client = Connector.GetHttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
            parameter: Session["TokenNumber"].ToString() + ":" + Session["UserName"]);

            var ticket = GetTicketDetails(id);
            if (ticket == null) return View("_NotFound");
            var req = Request.CurrentExecutionFilePath.Split('/');
            var urlTitle = req[req.Length - 1];
            if (Title != null && ticket.Title.GetHashCode() != Title)
            {
                Response.Status = "301 Moved Permanently";
                Response.StatusCode = 301;
                Response.AddHeader("Location", "/Ticket/Ticket/" + ticket.Id + "/" + ticket.Title.GetHashCode());
                Response.End();
            }

            return View(ticket);
        }

        // GET: Ticket
        public ActionResult Post()
        {
            try
            {
                var clientId = (int)Session["userId"];
                return View(new TicketSave { ClientId = clientId });
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex.Message);
                return HttpNotFound();
            }
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Post(TicketSave ticket, HttpPostedFileBase[] Files)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string url = "";
                    var client = GetHttpClientWithSession();

                    if (Files != null)
                    {
                        if (Files[0] != null)
                        {

                            ticket.Attachments = new string[Files.Length];
                            for (int i = 0; i < Files.ToList().Count; i++)
                            {
                                var file = Files.ToList()[i];
                                if (!(file.ContentType.Contains("image")))
                                {
                                    ModelState.AddModelError("", "Please select only image files");
                                    return View(ticket);
                                }
                                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                                //To-do check if the folder exist, if not create it 
                                var uploadPath = ConfigurationManager.AppSettings["UserAttachments"];
                                //var uploadPath = Server.MapPath("http:localhost:44323/Attachments/");
                                var fileExtension = Path.GetExtension(file.FileName);
                                fileName = DateTime.Now.ToString("yyyyMMdd") + "-" + fileName.Trim() + fileExtension;
                                ticket.Attachments[i] = uploadPath + fileName;
                                var path = ticket.Attachments[i];
                                //Save file to server folder 
                                if (Directory.Exists(uploadPath))
                                {
                                    Directory.CreateDirectory(uploadPath);
                                }
                                file.SaveAs(path);
                            }
                        }
                    }
                    var stringContent = Connector.GetStringOfObject(ticket);
                    if (ticket.Id != 0)
                    {
                        url = "ticket/EditTicket";
                    }
                    else
                    {
                        url = "ticket/postTicket";
                    }
                    var response = client.PostAsync(url, stringContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var httpResponseContent = response.Content.ReadAsStringAsync().Result;
                        var ticketId = new JavaScriptSerializer().Deserialize<int>(httpResponseContent);
                        return RedirectToAction("Ticket", new { id = ticketId, title = ticket.Title.GetHashCode() });
                    }
                    else
                    {
                        ModelState.AddModelError("", "server error please try later");
                    }

                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                ModelState.AddModelError("", "We're sorry! server error please try later");
            }
            return View(ticket);
        }

        public ActionResult All()
        {
            try
            {
               

                ViewBag.Count = GetTotal($"Dashboard/CountAllTickets?id={GetUserID()}");
                ViewBag.OpenTickets = GetTotal(TicketStatus.Opened);
                ViewBag.InprogressTickets = GetTotal(TicketStatus.Inprogress);
                ViewBag.ClosedTickets = GetTotal(TicketStatus.Closed);
                ViewBag.UnslovedTickets = GetTotal(TicketStatus.Unsolved);
                return View("DisplayAllTickets");
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex.Message);
                return HttpNotFound();
            }
        }
        [HttpPost]
        public ActionResult GetAllTickets()
        {
            try
            {

                PagingModel paging = new PagingModel
                {
                    DisplayStart = Convert.ToInt32(Request["start"]),
                    DisplayLength = Convert.ToInt32(Request["length"]),
                    SearchValue = Request["search[value]"],
                    FilterByPriority = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault(),
                    FilterByCategory = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault(),
                    FilterByStatus = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault(),
                    StartDate = Request["startDate"],
                    EndDate = Request["EndDate"],
                };

                List<TicketView> tickets = null;

                //Authentication
                var client = Connector.GetHttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                parameter: Session["TokenNumber"].ToString() + ":" + Session["UserName"]);

                client = GetHttpClientWithSession();
                HttpResponseMessage response = client.PostAsJsonAsync($"Ticket/GetTickets?id={GetUserID()}", paging).Result;

                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsAsync<DatatablePagination<TicketView>>().Result;
                    tickets = content.Data;
                    int totalRecord = content.TotalRecord;
                    int totalFilteredRecord = content.TotalFilteredRecord;

                    var result = from c in tickets
                                 select new[]{
                    Convert.ToString(c.Id),
                    Convert.ToString(c.Status),
                    Convert.ToString(c.Category),
                    Convert.ToString(c.Priority),
                    c.Title,
                    DateTime.Parse(c.OpenDate).ToString("dddd, dd MMMM yyyy hh:mm tt")};

                    return Json(new { aaData = result, recordsTotal = totalRecord, draw = Request["draw"], recordsFiltered = totalFilteredRecord }, JsonRequestBehavior.AllowGet);

                }
                else
                    return Json(new { success = false, message = "Somthing went wrong" });

            }

            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return Json(new { aaData = string.Empty.ToArray(), JsonRequestBehavior.AllowGet });
            }
        }
    }
}