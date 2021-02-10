using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using TicketingSystem.Common.Enums;
using TicketingSystem.Logging;
using TicketingSystem.Web.Employee.Filters;
using TicketingSystem.Web.Employee.Models;

namespace TicketingSystem.Web.Employee.Controllers
{
    [CustomeAuthorizationFilter("Manager")]
    public class DashboardController : BaseController
    {
        public ActionResult Dashboard()
        {
            try
            {
                //Authentication
                var client = Connector.GetHttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                parameter: Session["TokenNumber"].ToString() + ":" + Session["UserName"]);

                ViewBag.OpenTickets = GetTotal(TicketStatus.Opened);
                ViewBag.InprogressTickets = GetTotal(TicketStatus.Inprogress);
                ViewBag.ClosedTickets = GetTotal(TicketStatus.Closed);
                ViewBag.UnslovedTickets = GetTotal(TicketStatus.Unsolved);
                ViewBag.EmployeeCount = GetTotal("Dashboard/CountAllEmployees");
                ViewBag.ClientCount = GetTotal("Dashboard/CountAllClients");
                ViewBag.TicketCount = GetTotal("Dashboard/CountAllTickets");
                ViewBag.SaudiCount = GetTotal(UserCountry.Saudi);
                ViewBag.EgyptCount = GetTotal(UserCountry.Egypt);
                ViewBag.JordanCount = GetTotal(UserCountry.Jordan);
                ViewBag.EmiratesCount = GetTotal(UserCountry.Emirates);
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
            }
            return View();
        }

        public ActionResult TopFiveEmployee()
        {
            try
            {
                //Authentication
                var client = Connector.GetHttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme: "Bearer",
                parameter: Session["TokenNumber"].ToString() + ":" + Session["UserName"]);

                HttpResponseMessage response = client.GetAsync("Dashboard/TopFiveEmployees").Result;
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsAsync<IList<TopEmployeesModel>>().Result;
                    var result = from c in content
                                 select new[]{
                        Convert.ToString(c.Id),
                        c.FirstName,
                        c.LastName,
                        c.Email,
                       Convert.ToString(c.TicketsTotal)};

                    return Json(new
                    {
                        aaData = result,
                    },
                        JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
            }
            return Json(new { aaData = string.Empty }, JsonRequestBehavior.AllowGet);
        }
    }
}