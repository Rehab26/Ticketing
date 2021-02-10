using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Batch;
using System.Web.Http.Results;
using System.Web.Mvc;
using TicketingSystem.Logging;

namespace TicketingSystem.Web.Client.Controllers
{
    public class HealthCheckController : Controller
    {

        // Check MVC responsing by checking the counting files in the storage 
        public string PingMVC()
        {
            try
            {
                var path = ConfigurationManager.AppSettings["UserAttachments"];
                // searches the current directory and sub directory
                int fileCount = Directory.GetFiles(path, "*", SearchOption.AllDirectories).Length;
                if (fileCount > 0)
                    return "Storage Ok";
                else
                    return "Not Working or storage doesn't contain files yet";
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return "Error";
            }

        }


    }
}
