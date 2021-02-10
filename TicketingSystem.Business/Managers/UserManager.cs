using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Data;
using TicketingSystem.Common.Models.UserModels;
using TicketingSystem.Common.Enums;
using TicketingSystem.Common.Models.Paging;
using System.Collections;
using TicketingSystem.Logging;

namespace TicketingSystem.Business.Managers
{
    public class UserManager
    {

        public TicketingStystemContext db;
        public UserManager()
        {
            db = new TicketingStystemContext();
        }

        // Used for employees dropdown list
        public IEnumerable GetAllEmployees()
        {
            try
            {
                var entity = from u in db.Users
                             where (u.Type == UserType.Employee && u.UserStatus == 0)
                             select new ViewUser()
                             {
                                 EmployeeId = u.Id,
                                 FirstName = u.FirstName,
                                 LastName = u.LastName

                             };
                return entity.ToList();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        // List all Support Employees 
        public DatatablePagination<SupportEmployeesView> GetAllEmployees(PagingModel paging)
        {
            try
            {
                var employees = from a in db.Users.Where(i => i.Type == UserType.Employee)
                                select new SupportEmployeesView()
                                {
                                    Id = a.Id,
                                    Email = a.Email,
                                    FirstName = a.FirstName,
                                    LastName = a.LastName,
                                    DateOfBirth = a.DateOfBirth,
                                    PhoneNumber = a.PhoneNumber,
                                    UserStatus = a.UserStatus,
                                    // Type = a.Type
                                };
                if (employees != null)
                {
                    var user = new DatatablePagination<SupportEmployeesView>
                    {
                        Data = employees.ToList(),
                        TotalRecord = employees.Count(),
                    };
                    if (!string.IsNullOrEmpty(paging.SearchValue))
                    {
                        user.Data = user.Data
                                .Where(u => u.FirstName.ToLower().Contains(paging.SearchValue.ToLower()) ||
                                u.LastName.ToString().Contains(paging.SearchValue.ToLower()) ||
                                u.Email.ToString().Contains(paging.SearchValue.ToLower()) ||
                                u.PhoneNumber.ToLower().Contains(paging.SearchValue.ToLower())).ToList();
                    }
                    user.TotalFilteredRecord = user.Data.Count();
                    //  sorting not working with me ..
                    user.Data = user.Data.OrderBy(u => u.Id).ToList();
                    user.Data = user.Data.Skip(paging.DisplayStart).Take(paging.DisplayLength).ToList();
                    return user;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // List all clients with their ticket number 
        public DatatablePagination<ClientsWithTicketsView> GetClientsWithTickets(PagingModel paging)
        {
            try
            {
                var clients = from c in db.Users
                              join t in db.Tickets on c.Id equals t.ClientId
                              where c.Type == UserType.Client
                              group c by c.Id into clientGroups
                              select new
                              {
                                  Id = clientGroups.Key,
                                  Count = clientGroups.Count(),
                              };

                var clientsGroup = from u in db.Users
                                   join c in clients
                                   on u.Id equals c.Id
                                   select new ClientsWithTicketsView
                                   {
                                       Id = u.Id,
                                       FirstName = u.FirstName,
                                       LastName = u.LastName,
                                       Email = u.Email,
                                       PhoneNumber = u.PhoneNumber,
                                       DateOfBirth = u.DateOfBirth,
                                       UserStatus = u.UserStatus,
                                       TicketsTotal = c.Count
                                   };

                if (clientsGroup != null)
                {
                    var user = new DatatablePagination<ClientsWithTicketsView>
                    {
                        Data = clientsGroup.ToList(),
                        TotalRecord = clientsGroup.Count(),
                    };
                    if (!string.IsNullOrEmpty(paging.SearchValue))
                    {
                        user.Data = user.Data
                                .Where(u => u.FirstName.ToLower().Contains(paging.SearchValue.ToLower()) ||
                                u.LastName.ToString().Contains(paging.SearchValue.ToLower()) ||
                                u.Email.ToString().Contains(paging.SearchValue.ToLower()) ||
                                u.PhoneNumber.ToLower().Contains(paging.SearchValue.ToLower())).ToList();

                    }
                    user.TotalFilteredRecord = user.Data.Count();
                    //  sorting not working with me ..
                    user.Data = user.Data.OrderBy(u => u.Id).ToList();
                    user.Data = user.Data.Skip(paging.DisplayStart).Take(paging.DisplayLength).ToList();
                    return user;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        // List all clients with their ticket number 
        public DatatablePagination<ClientAndTicketCount> GetAllClients(PagingModel paging)
        {
            try
            {
                var all = (from u in db.Users.Where(u=> u.Type == UserType.Client)
                        .Select(s => new ClientAndTicketCount
                        {
                            ID = s.Id,
                            FirstName = s.FirstName,
                            LastName = s.LastName,
                            DateOfBirth = s.DateOfBirth,
                            Email = s.Email,
                            PhoneNumber = s.PhoneNumber,
                            UserStatus = s.UserStatus,
                            TicketsTotal = (db.Tickets.Where(t=>t.ClientId == s.Id).Count())
                        })
                    select u);
                if (all != null)
                {
                    var user = new DatatablePagination<ClientAndTicketCount>
                    {
                        Data = all.ToList(),
                        TotalRecord = all.Count(),
                    };
                    if (!string.IsNullOrEmpty(paging.SearchValue))
                    {
                        user.Data = user.Data
                            .Where(u => u.FirstName.ToLower().Contains(paging.SearchValue.ToLower()) ||
                                        u.LastName.ToString().Contains(paging.SearchValue.ToLower()) ||
                                        u.Email.ToString().Contains(paging.SearchValue.ToLower()) ||
                                        u.PhoneNumber.ToLower().Contains(paging.SearchValue.ToLower())).ToList();

                    }

                    user.TotalFilteredRecord = user.Data.Count();
                    //  sorting not working with me ..
                     //user.Data = user.Data.OrderBy(u => u.Id).ToList();
                     //user.Data = user.Data.Skip(paging.DisplayStart).Take(paging.DisplayLength).ToList();
                    return user;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // User Activation method
        public Boolean UserActivatation(int id)
        {
            var user = db.Users.Find(id);
            if (user == null)
            {
                return false;
            }
            else
            {
                if (user.UserStatus == UserStatus.Active)
                    user.UserStatus = UserStatus.Inactive;
                else if (user.UserStatus == UserStatus.Inactive)
                    user.UserStatus = UserStatus.Active;
                try
                {
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        // View user 
        public ViewUser GetUser(int id)
        {
            try
            {
                var entity = db.Users.Find(id);

                if (entity == null)
                {
                    return null;
                }
                else
                {
                    var user = new ViewUser()
                    {
                        FirstName = entity.FirstName,
                        LastName = entity.LastName,
                        Email = entity.Email,
                        PhoneNumber = entity.PhoneNumber,
                    };
                    return user;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ClientView GetClient(int id)
        {
            try
            {
                var entity = db.Users.Find(id);

                if (entity == null)
                {
                    return null;
                }
                else
                {
                    var user = new ClientView()
                    {
                        FirstName = entity.FirstName,
                        LastName = entity.LastName,
                        Id = entity.Id,
                        Email = entity.Email,
                        DateOfBirth = entity.DateOfBirth.Value.ToShortDateString(),
                        Country = entity.Country,
                        PhoneNumber = entity.PhoneNumber,
                        Type = entity.Type
                    };
                    return user;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Edit user information 
        public Boolean EditUser(int id, EditUserView editedUser)
        {
            var user = db.Users.Find(id);

            if (user == null)
            {
                return false;
            }
            else
            {
                user.FirstName = editedUser.FirstName;
                user.LastName = editedUser.LastName;
                user.DateOfBirth = editedUser.DateOfBirth;
                user.Email = editedUser.Email;
                user.PhoneNumber = editedUser.PhoneNumber;
                user.Password = editedUser.Password;

                try
                {
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

        }
        // Get manager information to use it in email
        public ViewUser GetManager()
        {
            try
            {
                var entity = (from a in db.Users.Where(a => a.Type == UserType.Manager)
                              select new ViewUser()
                              {
                                  FirstName = a.FirstName,
                                  LastName = a.LastName,
                                  Email = a.Email
                              }).FirstOrDefault();

                if (entity == null)
                {
                    return null;
                }
                else
                {
                    return entity;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
