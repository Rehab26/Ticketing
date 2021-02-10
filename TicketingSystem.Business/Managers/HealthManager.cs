using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Data;
using TicketingSystem.Logging;

namespace TicketingSystem.Business.Managers
{
    public class HealthManager
    {
        private TicketingStystemContext db = new TicketingStystemContext();

        //Total number of users
        public int CountUsers()
        {
            try
            {
                var users = from e in db.Users
                            select e;

                return users.Count();
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return 0;
            }
        }

        public int CountFiles()
        {
            try
            {
                var path = ConfigurationManager.AppSettings["UserAttachments"];
                // searches the current directory and sub directory
                int fileCount = Directory.GetFiles(path, "*", SearchOption.AllDirectories).Length;
                return fileCount;
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return 0;
            }
        }
    }
}

