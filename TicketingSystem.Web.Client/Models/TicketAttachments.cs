using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicketingSystem.Logging;

namespace TicketingSystem.Web.Client.Models
{
    public class TicketAttachments
    {
        public string[] Attachments { get; set; }
        public static string GetImage(string file)
        {
            try
            {
                byte[] byteData = System.IO.File.ReadAllBytes(file);
                string imreBase64Data = Convert.ToBase64String(byteData);
                string imgDataURL = string.Format("data:image/jpg;base64,{0}", imreBase64Data);
                return imgDataURL;
            }
            catch (Exception ex) {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return null;
            }
        }
    }
}