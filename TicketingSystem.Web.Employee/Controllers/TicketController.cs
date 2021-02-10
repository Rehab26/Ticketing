using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using TicketingSystem.Common.Models.Paging;
using TicketingSystem.Logging;
using TicketingSystem.Web.Employee.Models;
using TicketingSystem.Common.Enums;
using System.Configuration;
using System.IO;
using System.Web;
using TicketingSystem.Web.Employee.Filters;

namespace TicketingSystem.Web.Employee.Controllers
{
    [Log]
    public class TicketController : BaseController
    {
        [CustomeAuthorizationFilter("Employee")]
        public ActionResult DisplayEmployeeTickets()
        {
            try
            {
                //Authentication
                var client = Connector.GetHttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                parameter: Session["TokenNumber"].ToString() + ":" + Session["UserName"].ToString());

                ViewBag.Count = GetTotal($"Dashboard/CountAllTickets?id={GetUserID()}");
                ViewBag.InprogressTickets = GetTotal(TicketStatus.Inprogress, GetUserID());
                ViewBag.ClosedTickets = GetTotal(TicketStatus.Closed, GetUserID());
                ViewBag.UnslovedTickets = GetTotal(TicketStatus.Unsolved, GetUserID());
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
            }

            return View();
        }

        [CustomeAuthorizationFilter("Manager")]

        public ActionResult DisplayAllTickets()
        {
            return View();
        }

        // allow manager to change ticket status
        [CustomeAuthorizationFilter("Manager", "Employee")]
        public ActionResult UpdateTicket(int id = 0)
        {
            try
            {
                if (id != 0)
                {
                    //Authentication
                    var client = Connector.GetHttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: Session["TokenNumber"].ToString() + ":" + Session["UserName"]);

                    // Get single user 
                    HttpResponseMessage response = client.GetAsync($"Ticket/ViewTicket?id={id}").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var ticket = response.Content.ReadAsAsync<TicketSaveModel>().Result;

                        return View(ticket);
                    }

                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                ViewBag.Message = $"Oops! Something went wrong, please try again later";
            }
            return View();
        }

        [HttpPost]
        [CustomeAuthorizationFilter("Employee")]
        public ActionResult GetEmployeeTickets()
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

                List<TicketViewModel> tickets = null;
                //Authentication
                var client = Connector.GetHttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                parameter: Session["TokenNumber"].ToString() + ":" + Session["UserName"]);
                HttpResponseMessage response = client.PostAsJsonAsync<PagingModel>($"Ticket/GetTickets?id={GetUserID()}", paging).Result;

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var content = response.Content.ReadAsAsync<DatatablePagination<TicketViewModel>>().Result;
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
                    c.OpenDateFormatted};

                        return Json(new { aaData = result, recordsTotal = totalRecord, draw = Request["draw"], recordsFiltered = totalFilteredRecord }, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception ex)
                    {
                        GlobalVariable.log.Write(LogLevel.Error, ex);
                        return Json(new { success = false, message = "No records in table" });
                    }

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
        [CustomeAuthorizationFilter("Manager")]
        public ActionResult GetAllTickets()
        {
            try
            {
                PagingModel paging = new PagingModel
                {
                    // server side parameter
                    DisplayStart = Convert.ToInt32(Request["start"]),
                    DisplayLength = Convert.ToInt32(Request["length"]),
                    SearchValue = Request["search[value]"],
                    FilterByPriority = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault(),
                    FilterByCategory = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault(),
                    FilterByStatus = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault(),
                    StartDate = Request["startDate"],
                    EndDate = Request["EndDate"],
                };

                List<TicketViewModel> tickets = null;
                //Authentication
                var client = Connector.GetHttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                parameter: Session["TokenNumber"].ToString() + ":" + Session["UserName"]);

                HttpResponseMessage response = client.PostAsJsonAsync<PagingModel>($"Ticket/GetTickets?id=0", paging).Result;

                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsAsync<DatatablePagination<TicketViewModel>>().Result;
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
                      c.OpenDateFormatted,
                                 c.EmployeeName,
                                 c.ClientName};

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
        [CustomeAuthorizationFilter("Manager")]
        public ActionResult TicketAssignment(int id = 0)
        {
            try
            {
                //Authentication
                var client = Connector.GetHttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                parameter: Session["TokenNumber"].ToString() + ":" + Session["UserName"]);

                HttpResponseMessage response = client.GetAsync("User/GetAllEmployee").Result;
                var items = response.Content.ReadAsAsync<IList<TicketAssignment>>().Result;

                if (items != null)
                {
                    ViewBag.data = items;
                }

                if (id != 0)
                {
                    // Get single user 
                    HttpResponseMessage httpResponse = client.GetAsync($"Ticket/GetTiecktByID?id={id}").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var ticket = httpResponse.Content.ReadAsAsync<TicketAssignment>().Result;
                        return View(ticket);
                    }

                }

            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
            }
            return View();
        }

        [CustomeAuthorizationFilter("Manager", "Employee")]
        public ActionResult UpdateTicektStatus(TicketSaveModel ticket)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Authentication
                    var client = Connector.GetHttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: Session["TokenNumber"].ToString() + ":" + Session["UserName"]);

                    if (ticket.Status == TicketStatus.Closed) {
                        ticket.EmployeeId = GetUserID();
                    }

                    HttpResponseMessage response = client.PutAsJsonAsync<TicketSaveModel>($"Ticket/EditTicketStatus", ticket).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(new { success = true, message = "Updated" });
                    }

                }
                return Json(new { success = true, message = "Something went wrong" });
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return Json(new { success = false, message = "Oops! Something went wrong, please try again later" }, JsonRequestBehavior.AllowGet);
            }
        }
        [CustomeAuthorizationFilter("Manager")]
        public ActionResult AssignTicket(TicketAssignment ticket)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Authentication
                    var client = Connector.GetHttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: Session["TokenNumber"].ToString() + ":" + Session["UserName"]);

                    HttpResponseMessage response = client.PutAsJsonAsync<TicketAssignment>($"Ticket/AssignTicket", ticket).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(new { success = true, message = "Successfully Assigned" });
                    }

                }
                return Json(new { success = false, message = "Something went wrong" });
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return Json(new { success = false, message = "Oops! Something went wrong, please try again later" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        [CustomeAuthorizationFilter("Manager", "Employee")]
        public ActionResult AddComment(ReplyModel reply, HttpPostedFileBase File)
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
                        //Save file to server folder 
                        file.SaveAs(path);
                    }
                    // reply.UserId = GetUserID();
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

        [HttpGet]
        [CustomeAuthorizationFilter("Manager", "Employee")]
        public ActionResult Ticket(int id, int? Title)
        {
            try
            {
                //Authentication
                var client = Connector.GetHttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                parameter: Session["TokenNumber"].ToString() + ":" + Session["UserName"]);

                var ticket = GetTicketDetails(id);
                if (ticket == null) return View("Error");
                var req = Request.CurrentExecutionFilePath.Split('/');
                if (Title != null && ticket.Title.GetHashCode() != Title)
                {
                    Response.Status = "301 Moved Permanently";
                    Response.StatusCode = 301;
                    Response.AddHeader("Location", "/Ticket/Ticket/" + ticket.Id + "/" + ticket.Title.GetHashCode());
                    Response.End();
                }

                return View(ticket);
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return View("_NotFound");
            }
        }

    }
}