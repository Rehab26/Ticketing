using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Common.Models.Email;
using TicketingSystem.Common.Models.TicketModels;
using TicketingSystem.Common.Models.UserModels;
using TicketingSystem.Logging;

namespace TicketingSystem.Business.Managers
{
    public class EmailManager
    {

        // Send email to manager after getting new ticket in the system
        public void NewTicketEmail(TicketSave ticket , string ticketFor)
        {
            try
            {
                // Get user information to send email
                UserManager userManager = new UserManager();
                IEnumerable<ViewUser> manager = null;
                ViewUser user = null;


                if (ticketFor == "Manager")
                {
                    user = userManager.GetManager();
                }
                else if (ticketFor == "Employee")
                {
                    user = userManager.GetUser((int)ticket.EmployeeId);
                }

                EmailModel emailModel = new EmailModel();
                emailModel.toname = user.FirstName + " " + user.LastName;
                emailModel.toemail = user.Email;
                emailModel.subject = "No Reply: New Ticket! ";
                var imgUrl = ConfigurationManager.AppSettings["LogoImgUrlForEmail"];
                //var url = ConfigurationManager.AppSettings["EmployeeDomain"] + "/Ticket/Ticket/";
                emailModel.message =
                      "<div style='Text-align:center;background-color:#f9fbfd'>"
                    + $"<img src='{imgUrl}' alt='logo'/>"
                    + "<h2> Dear " + emailModel.toname + ", <br>You have a new ticket!</h2><hr/>"
                    + "<h2> Title: " + ticket.Title + "</h2>"
                    + "<h3>" + ticket.Category + " | " + ticket.Priority + "</h3>"
                    + "<div></div>"
                    + "<h5>" + ticket.Description + ".</h5> <hr/>"
                    + "</div>";

                // Send mail to employee with new ticket assigned to him/her
                try
                {
                    EmailManager emailManager = new EmailManager();
                    emailManager.SendEmail(emailModel);
                }
                catch (Exception ex)
                {
                    GlobalVariable.log.Write(LogLevel.Error, ex);
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
            }
        }

        // Send email to client/employee after each reply
        public void ReplyEmail(ReplyModel reply, string ticketFor)
        {
            try
            {
                // Get user information to send email
                UserManager userManager = new UserManager();
                TicketManager ticketManager = new TicketManager();
                ViewUser user = null;
                ViewUser replyUser = userManager.GetUser(reply.UserId); ;
                var ticket = ticketManager.GetSpecificTieckt(reply.TicketId);

                if (ticketFor == "Client")
                {
                    user = userManager.GetUser(ticket.ClientId);
                }
                else if (ticketFor == "Employee")
                {
                    user = userManager.GetUser(ticket.EmployeeId);
                    if (user == null)
                    {
                        user = userManager.GetManager();
                    }
                }

                EmailModel emailModel = new EmailModel();
                emailModel.toname = user.FirstName + " " + user.LastName;
                emailModel.toemail = user.Email;
                var imgUrl = ConfigurationManager.AppSettings["LogoImgUrlForEmail"];
                emailModel.subject = "You've Received New Reply from " + replyUser.FirstName+" "+replyUser.LastName + "! ";
                emailModel.message =
                      "<div style='Text-align:center;background-color:#f9fbfd'>"
                    + $"<img src='{imgUrl}' alt='logo'/>"
                    + "<h2> Dear " + emailModel.toname + ","
                    + "<br>"+ replyUser.FirstName + " " + replyUser.LastName + " reply to your ticket!</h2><hr/>"
                    + "<h2> Ticket: " + ticket.Title + "</h2>"
                    + "<h3>" + ticket.Category + " | " + ticket.Priority + "</h3>"
                    + "<div></div>"
                    + "<h4>Reply: " + reply.Content + ".</h4> <hr/>"
                    + "</div>";

                // Send mail to employee/clien to notify about the reply
                try
                {
                    EmailManager emailManager = new EmailManager();
                    emailManager.SendEmail(emailModel);
                }
                catch (Exception ex)
                {
                    GlobalVariable.log.Write(LogLevel.Error, ex);
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
            }
        }

        // Send email to change password for user
        public void ForgetPsswordEmail(PasswordForget changing)
        {
            try
            {
                EmailModel emailModel = new EmailModel();
                emailModel.toname = "Client";
                emailModel.toemail = changing.Email;
                emailModel.subject = "No Reply: Password Reset Request! ";
                var url = changing.Link; 
                emailModel.message =
                      "<div style='Text-align:center;background-color:#f9fbfd'>"
                    + "<img src='https://i.ibb.co/6g1vBwY/logo-text.png' alt='logo'/>"
                    + "<h2> Dear Client, </h2>"
                    + "<h4> You recently requested to reset your password for your account. Click the link below to reset it.</h4>"
                    + "<h4> <br/><a href=" + url + ">" + "click here" + "</a> <br/></h4>"
                    + "<h4> If you did not request a password reset, please ignore this email or reply to let us know.<br/><br/> Thank you.</h4>"
                    + "</div>";

                // Send mail to client
                try
                {
                    EmailManager emailManager = new EmailManager();
                    emailManager.SendEmail(emailModel);
                }
                catch (Exception ex)
                {
                    GlobalVariable.log.Write(LogLevel.Error, ex);
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                throw ex;
            }
        }

        // Send email to manager/employee/client
        public void SendEmail(EmailModel emailModel)
        {
            try
            {
                // Take business email & password from configuration
                var Bemail = ConfigurationManager.AppSettings["BusinessEmail"];
                var Bpassword = ConfigurationManager.AppSettings["BusinessPassword"];

                var fromEmail = Bemail; // Email system
                var toEmail = new MailAddress(emailModel.toemail);
                var fromEmailPassword = Bpassword; // email system password
                string subject = emailModel.subject;
                string body = emailModel.message;

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromEmail, fromEmailPassword)
                };

                using (var message = new MailMessage(fromEmail, toEmail.ToString())
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                    smtp.Send(message);

            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                throw ex;
            }
        }
    }
}
