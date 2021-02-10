using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketingSystem.Logging;

namespace TicketingSystem.Web.Employee.Controllers
{
    public class PhotoController : Controller
    {
        // GET: Photo        
        public ActionResult TakePhoto() {
            try
            {
                Session["filePath"] = "";
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
            }
            return View();
        }

        public ActionResult Rebind()
        {
            try
            {
                string path = Session["filePath"].ToString();
                byte[] byteData = System.IO.File.ReadAllBytes(path);
                string imreBase64Data = Convert.ToBase64String(byteData);
                string imgDataURL = string.Format("data:image/jpg;base64,{0}", imreBase64Data);
                return Json(imgDataURL, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return null;
            }
        }


        public ActionResult Capture()
        {
            try
            {
                var stream = Request.InputStream;
                string dump;

                using (var reader = new StreamReader(stream))
                {
                    dump = reader.ReadToEnd();

                    DateTime nm = DateTime.Now;

                    string date = nm.ToString("yyyymmddMMss");

                    var path = ConfigurationManager.AppSettings["UserAttachments"] + date + ".jpg";

                    System.IO.File.WriteAllBytes(path, String_To_Bytes2(dump));

                    ViewData["path"] = date + ".jpg";

                    Session["filePath"] = path;
                }
            }
            catch (Exception ex) {
                // path not found 
                GlobalVariable.log.Write(LogLevel.Error,ex);
            }
            return View("TakePhoto");
        }

        private byte[] String_To_Bytes2(string strInput)
        {
            try
            {
                int numBytes = (strInput.Length) / 2;

                byte[] bytes = new byte[numBytes];

                for (int x = 0; x < numBytes; ++x)
                {
                    bytes[x] = Convert.ToByte(strInput.Substring(x * 2, 2), 16);
                }

                return bytes;
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return null;
            }
        }

        public ActionResult LoadImage(string path)
        {
            try
            {
                return File(path, "application/jpg");
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return null;
            }
        }
    }
}