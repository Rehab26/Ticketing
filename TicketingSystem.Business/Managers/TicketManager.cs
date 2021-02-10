using System;
using System.Collections.Generic;
using System.Linq;
using TicketingSystem.Common.Enums;
using TicketingSystem.Common.Models.Paging;
using TicketingSystem.Common.Models.TicketModels;
using TicketingSystem.Common.Models.UserModels;
using TicketingSystem.Data;
using TicketingSystem.Data.Entites;
using TicketingSystem.Logging;

namespace TicketingSystem.Business.Managers
{
    public class TicketManager
    {
      
        public TicketingStystemContext db;
        public TicketManager()
        {
            db = new TicketingStystemContext();
        }
        public ViewTicket GetSpecificTieckt(int id)
        {
            try
            {
                var entity = (from t in db.Tickets
                              where id == t.Id
                              select new ViewTicket()
                              {
                                  Id = t.Id,
                                  Status = t.Status,
                                  Category = t.Category,
                                  Priority = t.Priority,
                                  Title = t.Title,
                                  Description = t.Description,
                                  OpenDate = t.OpenDate,
                                  ClosedDate = t.ClosedDate,
                                  EmployeeId = t.EmployeeId ?? 0,
                                  ClientId = t.ClientId
                              });
                if (entity != null)
                {
                    return entity.FirstOrDefault();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                throw ex;
            }
        }

        // List all tikets with status for Manager
        public DatatablePagination<ViewTicket> GetAllTickets( int id,PagingModel paging)
        {
            try
            {
                IQueryable<ViewTicket> tickets = null;
                if (id == 0)
                {
                    tickets = from t in db.Tickets
                              join u in db.Users on t.ClientId equals u.Id
                              join e in db.Users on t.EmployeeId equals e.Id into ulist
                              from e in ulist.DefaultIfEmpty()
                              select new ViewTicket()
                              {
                                  Id = t.Id,
                                  Status = t.Status,
                                  Category = t.Category,
                                  Priority = t.Priority,
                                  Title = t.Title,
                                  ClientId = t.ClientId,
                                  EmployeeId = t.EmployeeId ?? 0,
                                  OpenDate = t.OpenDate,
                                  ClosedDate = t.ClosedDate,
                                  ClosedBy = t.ClosedBy ?? 0,
                                  ClientName = u.FirstName + " " + u.LastName,
                                  EmployeeName = e.FirstName + " " + e.LastName,
                              };
                }

                else {
                     tickets = from t in db.Tickets.Where(i => i.EmployeeId == id || i.ClientId == id)
                               join u in db.Users on t.ClientId equals u.Id
                               join e in db.Users on t.EmployeeId equals e.Id into ulist
                               from e in ulist.DefaultIfEmpty()
                               select new ViewTicket()
                                  {
                                      Id = t.Id,
                                      Status = t.Status,
                                      Category = t.Category,
                                      Priority = t.Priority,
                                      Title = t.Title,
                                      OpenDate = t.OpenDate,
                                      EmployeeId = t.EmployeeId ?? 0,
                                      ClientId = t.ClientId,
                                   ClientName = u.FirstName + " " + u.LastName,
                                   EmployeeName = e.FirstName + " " + e.LastName,
                               };
                }
                if (tickets != null)
                {
                    var ticket = new DatatablePagination<ViewTicket>
                    {
                        Data = tickets.ToList(),
                        TotalRecord = tickets.Count(),
                    };
                    if (!string.IsNullOrEmpty(paging.SearchValue))
                    {

                        ticket.Data = ticket.Data
                                .Where(u => u.Title.ToLower().Contains(paging.SearchValue.ToLower()) ||
                                u.Id.ToString().Contains(paging.SearchValue.ToLower()) ||
                                u.EmployeeName.ToLower().Contains(paging.SearchValue.ToLower()) ||
                                u.ClientName.ToLower().Contains(paging.SearchValue.ToLower())).ToList();
                    }
                    if (!string.IsNullOrEmpty(paging.FilterByStatus) ||
                        !string.IsNullOrEmpty(paging.FilterByPriority) ||
                        !string.IsNullOrEmpty(paging.FilterByCategory)
                        )
                    {
                        ticket.Data = ticket.Data.Where(u => u.Status.ToString().Contains(paging.FilterByStatus)
                        && u.Priority.ToString().Contains(paging.FilterByPriority)
                        && u.Category.ToString().Contains(paging.FilterByCategory)).ToList();
                    }

                    if (!string.IsNullOrEmpty(paging.StartDate) && !string.IsNullOrEmpty(paging.EndDate))
                    {
                        ticket.Data = ticket.Data.Where(x => x.OpenDate >= DateTime.Parse(paging.StartDate) && x.OpenDate <= DateTime.Parse(paging.EndDate)).ToList();
                    }
                    ticket.TotalFilteredRecord = ticket.Data.Count();
                    ticket.Data = ticket.Data.OrderByDescending(u => u.OpenDate).ToList();
                    ticket.Data = ticket.Data.Skip(paging.DisplayStart).Take(paging.DisplayLength).ToList();
                    return ticket;
                }
                else
                {
                    return null;
                }
            }

            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return null;
            }
        }

        public IList<ViewEmployeeTickets> GetClientTickets(int id)
        {
            try
            {
                var tickets = from t in db.Tickets.Where(i => i.ClientId == id)
                              select new ViewEmployeeTickets()
                              {
                                  Id = t.Id,
                                  Status = t.Status,
                                  Category = t.Category,
                                  Priority = t.Priority,
                                  Title = t.Title,
                                  OpenDate = t.OpenDate,
                                  ClientId = (int)t.ClientId,
                                  Description = t.Description,

                              };
                return tickets.OrderByDescending(s => s.OpenDate).Take(5).ToList();
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                throw ex;
            }
        }

        // List all tickets with their replys based on ticket ID : 
        // 1 - View ticket information
        public TicketView GetTicket(int id, int userId)
        {
            try
            {
                var user = db.Users.Find(userId);
                var entity = db.Tickets.Find(id);
                var client = db.Users.Find(entity.ClientId);
                var ticket = new TicketView();
                if ((user.Type == UserType.Client && entity.ClientId == user.Id) ||
                    (user.Type == UserType.Employee && entity.EmployeeId == user.Id) ||
                    user.Type == UserType.Manager)
                {
                    if (entity.Status == TicketStatus.Closed)
                    {
                        ticket.ClosedDate = entity.ClosedDate.Value.ToShortDateString();
                        var closeBy = db.Users.Find(entity.ClosedBy);
                        ticket.ClosedBy = closeBy.FirstName + " " + closeBy.LastName;
                    }
                    else ticket.ClosedDate = null;
                    ticket.Id = entity.Id;
                    ticket.Client = client.FirstName + " " +client.LastName;
                    ticket.Category = entity.Category;
                    ticket.Status = entity.Status;
                    ticket.OpenDate = entity.OpenDate.ToShortDateString();
                    ticket.Title = entity.Title;
                    ticket.Description = entity.Description;
                    return ticket;
                }
                return null;
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return null;
            }
        }
        public TicketAttachments GetTicketAttachment(int id)
        {
            try
            {
                var files = new TicketAttachments();
                var filesEntity = db.FileStorages.Where(f => f.Reference == id && f.Type == AttachmentType.Ticket).ToList();
                if (filesEntity != null)
                {
                    if (filesEntity.Count != 0)
                    {
                        var count = filesEntity.Count;
                        files.Attachments = new string[count];
                        for (int i = 0; i < filesEntity.Count; i++)
                        {
                            files.Attachments[i] = filesEntity[i].Path;
                        };
                    }
                    return files;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
            }
            return null;
        }
        public bool ViewTicket(int ticketId)
        {
            ViewTicket viewticket = null;

            try
            {
                var entity = db.Tickets.Find(ticketId);

                if (entity == null)
                {
                    return false;
                }
                else
                {
                    var ticket = from t in db.Tickets.Where(i => i.Id == ticketId)
                                 join u in db.Users.DefaultIfEmpty() on t.ClientId equals u.Id
                                 select new ViewTicket()
                                 {
                                     Id = t.Id,
                                     Status = t.Status,
                                     Category = t.Category,
                                     Priority = t.Priority,
                                     Title = t.Title,
                                     Description = t.Description,
                                     ClientName = u.FirstName + " " + u.LastName,
                                     OpenDate = t.OpenDate,
                                     ClosedDate = t.ClosedDate,
                                     ClosedByEmployee = u.FirstName + " " + u.LastName
                                 };

                    // View attachments 
                    viewticket = (ViewTicket)ticket;
                    if (viewticket.Attachments.Length > 0)
                    {
                        foreach (var t in viewticket.Attachments)
                        {
                            var attach = from f in db.FileStorages.Where(f => f.Type == AttachmentType.Ticket && f.Reference == ticketId)
                                         select new ViewAttachment()
                                         {
                                             Path = f.Path
                                         };
                        }
                    }

                    // View replys
                    if (entity.Replys != null)
                    {
                        foreach (var r in entity.Replys)
                        {
                            var reply = from rp in db.Replys.Where(rp => rp.TicketId == ticketId)
                                        select new ViewReply()
                                        {
                                            Content = rp.Content,
                                            Time = rp.Time
                                        };
                        }
                    }


                    return true;
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return false;
            }
        }

        // 2- View replys for specific ticket
        public IQueryable<ReplyModel> ViewReplys(int ticketId)
        {
            try
            {
                var entities = db.Replys.Where(rep => rep.TicketId == ticketId).ToList();
                List<FileStorage> files = new List<FileStorage>();

                foreach (Reply entity in entities)
                {
                    var file = db.FileStorages.Where(s => s.Reference == entity.Id && s.Type == AttachmentType.Reply).FirstOrDefault();
                    if (file != null) files.Add(file);
                }
                //var test = files.Find(f => f.Path != null && f.Type == AttachmentType.Reply).Equals(null);
                var images = from f in files where f != null && f.Path != null select f;
                //files.Find(f=> f.Path!=null && s.Id == f.Reference && f.Type == AttachmentType.Reply).Path
                IQueryable<ReplyModel> replys;
                if (files.Count != 0)
                {
                    replys = (from r in entities.Where(r => r.TicketId == ticketId)
                             .Select(s => new ReplyModel
                             {
                                 Content = s.Content,
                                 Time = s.Time,
                                 UserId = s.UserId,
                                 TicketId = s.TicketId,
                                 Attachment = ((from f in files
                                                where f != null && f.Path != null && s.Id == f.Reference && f.Type == AttachmentType.Reply
                                                select f.Path).FirstOrDefault()),
                                 User = new ViewUser
                                 {
                                     Id = s.UserId,
                                     FirstName = s.User.FirstName,
                                     LastName = s.User.LastName,
                                 }
                                 ,
                             }
                             )
                              select r)
                             .AsQueryable();
                    replys.OrderBy(s => s.Time);

                }
                else
                {


                    replys = (from r in entities.Where(r => r.TicketId == ticketId)
                                 .Select(s => new ReplyModel
                                 {
                                     Content = s.Content,
                                     Time = s.Time,
                                     UserId = s.UserId,
                                     TicketId = s.TicketId,

                                     User = new ViewUser
                                     {
                                         Id = s.UserId,
                                         FirstName = s.User.FirstName,
                                         LastName = s.User.LastName,
                                     }
                                 }
                                 )
                              select r)
                                 .AsQueryable();
                }
                return replys.OrderBy(s => s.Time);
            }

            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return null;
            }
        }


        // Add new ticket
        public int AddTicket(TicketSave ticket)
        {
            var tick = new Ticket()
            {
                Status = TicketStatus.Opened,
                Category = ticket.Category,
                Priority = ticket.Priority,
                Title = ticket.Title,
                Description = ticket.Description,
                OpenDate = DateTime.Now,
                ClientId = ticket.ClientId,
            };
            try
            {
                db.Tickets.Add(tick);
                db.SaveChanges();

                if (ticket.Attachments != null)
                {
                    foreach (string path in ticket.Attachments)
                    {
                        db.FileStorages.Add(new FileStorage()
                        {
                            Path = path,
                            Reference = tick.Id,
                            Type = AttachmentType.Ticket,
                            CreateDate = DateTime.Now
                        });
                        db.SaveChanges();
                    }
                }

                // Update model info
                ticket.Id = tick.Id;
                //Send email to manager after submitting new ticket to the system
                EmailManager emailManager = new EmailManager();
                emailManager.NewTicketEmail(ticket, "Manager");

                return tick.Id;
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return 0;
            }
        }

        //Add reply and attachments
        public bool AddReply(ReplyModel reply)
        {
            try
            {
                var ticket = db.Tickets.Find(reply.TicketId);
                var user = db.Users.Find(reply.UserId);
                //If ticket close no one can add replay to it
                if (ticket.Status == TicketStatus.Closed) return false;
                //Who can add replay?
                //1- ticket belong to client OR 
                //2- it's assign to one employee OR 
                //3- the user is manager, 
                if (user.Id == ticket.ClientId || user.Id == ticket.EmployeeId || user.Type == UserType.Manager)
                {
                    var entity = new Reply()
                    {
                        TicketId = reply.TicketId,
                        Content = reply.Content,
                        Time = DateTime.Now,
                        UserId = reply.UserId
                    };
                    db.Replys.Add(entity);
                    db.SaveChanges();

                    if (reply.Attachment != null)
                    {
                        db.FileStorages.Add(new FileStorage()
                        {
                            Type = AttachmentType.Reply,
                            Path = reply.Attachment,
                            Reference = entity.Id,
                            CreateDate = entity.Time
                        });
                        db.SaveChanges();
                    }

                    // Send email to notify user about reply
                    EmailManager emailManager = new EmailManager();
                    if (user.Type == UserType.Client)
                        emailManager.ReplyEmail(reply, "Employee");
                    else
                        emailManager.ReplyEmail(reply, "Client");

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return false;
            }
        }

        // Method to edit ticket status to be used by employee and manager
        public Boolean EditTicketStatus(TicketSave ticket)
        {
            try
            {
                var entity = db.Tickets.Find(ticket.Id);

                if (entity == null)
                {
                    return false;
                }
                else
                {
                    entity.Status = ticket.Status;
                    if (entity.Status == TicketStatus.Closed)
                    {
                        entity.ClosedDate = DateTime.Now;
                        entity.ClosedBy = ticket.EmployeeId; // null
                    }

                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return false;
            }
        }

        // to be used by client
        public int EditTicket(TicketSave ticket)
        {
            try
            {
                var entity = db.Tickets.Find(ticket.Id);

                if (entity == null)
                {
                    return 0;
                }
                else
                {
                    entity.Category = ticket.Category;
                    entity.Priority = ticket.Priority;
                    entity.Title = ticket.Title;
                    entity.Description = ticket.Description;
                    db.SaveChanges();
                    return entity.Id;
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return 0;
            }
        }

        // Assign ticket to an employee
        public bool AssignTicketToEmployee(TicketSave ticket)
        {
            try
            {
                var entity = db.Tickets.Find(ticket.Id);

                if (entity == null)
                {
                    return false;
                }
                else
                {
                    entity.EmployeeId = ticket.EmployeeId;
                    entity.Status = TicketStatus.Inprogress;
                    db.SaveChanges();
                    // Send email to employee after assining
                    EmailManager emailManager = new EmailManager();
                    emailManager.NewTicketEmail(ticket, "Employee");

                    return true;
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return false;
            }
        }
    }
}
