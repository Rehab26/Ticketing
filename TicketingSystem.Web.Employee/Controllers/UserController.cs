using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using TicketingSystem.Web.Employee.Models;
using TicketingSystem.Common.Models.Paging;
using TicketingSystem.Web.Employee.Filters;
using TicketingSystem.Logging;
using System.Net.Http.Headers;

namespace TicketingSystem.Web.Employee.Controllers
{
    [CustomeAuthorizationFilter("Manager")]
    [Log]
    public class UserController : BaseController
    {
        // GET: User
        public ActionResult DisplayAllClients()
        {
            return View();
        }

        public ActionResult DisplayAllEmployee()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetClients()
        {
            try
            {
                PagingModel paging = new PagingModel
                {
                    // server side parameter
                    DisplayStart = Convert.ToInt32(Request["start"]),
                    DisplayLength = Convert.ToInt32(Request["length"]),
                    SearchValue = Request["search[value]"],
                };
                List<UserViewModel> clients = null;

                //Authentication
                var client = Connector.GetHttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                parameter: Session["TokenNumber"].ToString() + ":" + Session["UserName"]);

                HttpResponseMessage response = client.PostAsJsonAsync<PagingModel>("User/GetAllClients", paging).Result;
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsAsync<DatatablePagination<UserViewModel>>().Result;
                    clients = content.Data;
                    int totalRecord = content.TotalRecord;
                    int totalFilteredRecord = content.TotalFilteredRecord;
                    var result = from c in clients
                                 select new[]{
                        Convert.ToString(c.Id),
                        c.FirstName,
                        c.LastName,
                        c.Email,
                        c.DateOfBirthFormatted,
                        c.PhoneNumber,
                        Convert.ToString(c.TicketsTotal),
                        Convert.ToString(c.UserStatus),};

                    return Json(new
                    {
                        aaData = result,
                        recordsTotal = totalRecord,
                        draw = Request["draw"],
                        recordsFiltered = totalFilteredRecord
                    },
                        JsonRequestBehavior.AllowGet);
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

        public ActionResult GetSupportEmployee()
        {
            try
            {
                PagingModel paging = new PagingModel
                {
                    // server side parameter
                    DisplayStart = Convert.ToInt32(Request["start"]),
                    DisplayLength = Convert.ToInt32(Request["length"]),
                    SearchValue = Request["search[value]"],
                };
                List<UserViewModel> clients = null;

                //Authentication
                var client = Connector.GetHttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                parameter: Session["TokenNumber"].ToString() + ":" + Session["UserName"]);

                HttpResponseMessage response = client.PostAsJsonAsync<PagingModel>("User/GetAllEmployees", paging).Result;
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsAsync<DatatablePagination<UserViewModel>>().Result;
                    clients = content.Data;
                    int totalRecord = content.TotalRecord;
                    int totalFilteredRecord = content.TotalFilteredRecord;
                    var result = from c in clients
                                 select new[]{
                        Convert.ToString(c.Id),
                        c.FirstName,
                        c.LastName,
                        c.Email,
                        c.DateOfBirthFormatted,
                        c.PhoneNumber,
                        Convert.ToString(c.UserStatus)};

                    return Json(new
                    {
                        aaData = result,
                        recordsTotal = totalRecord,
                        draw = Request["draw"],
                        recordsFiltered = totalFilteredRecord
                    },
                        JsonRequestBehavior.AllowGet);
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

        public ActionResult UpdateUser(int id = 0)
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
                    HttpResponseMessage response = client.GetAsync($"user/get?id={id}").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var user = response.Content.ReadAsAsync<UserViewModel>().Result;

                        return View(user);
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

        public ActionResult UpdateUserStatus(UserViewModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Authentication
                    var client = Connector.GetHttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                    parameter: Session["TokenNumber"].ToString() + ":" + Session["UserName"]);

                    HttpResponseMessage response = client.GetAsync($"User/UserActivation?id={user.Id}").Result;
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
    }
}