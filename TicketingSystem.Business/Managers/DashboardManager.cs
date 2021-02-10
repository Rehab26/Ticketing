using System;
using System.Collections.Generic;
using System.Linq;
using TicketingSystem.Common.Enums;
using TicketingSystem.Common.Models.UserModels;
using TicketingSystem.Data;
using TicketingSystem.Logging;

namespace TicketingSystem.Business.Managers
{
    public class DashboardManager
    {
        public TicketingStystemContext db;
        public DashboardManager()
        {
            db = new TicketingStystemContext();
        }

        //Number of total employees
        public int GetAllEmployeesCount()
        {         
            try
            {
                var employees = from e in db.Users.Where(t => t.Type == UserType.Employee)
                                select e;

                return employees.Count();
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return 0;
            }
        }

        //Number of total clients
        public int GetAllClientsCount()
        {          
            try
            {
                var clients = from c in db.Users.Where(t => t.Type == UserType.Client)
                              select c;

                return clients.Count();
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return 0;
            }
        }

        //Number of total tickets
        public int GetAllTicketsCount(int id = 0)
        {

            try
            {
                if (id != 0)
                {
                    var tickets = (from t in db.Tickets.Where(s => s.ClientId == id || s.EmployeeId == id) select t);
                    return tickets.Count();
                }
                else
                {
                    var tickets = from t in db.Tickets
                                  select t;
                    return tickets.Count();
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return 0;
            }
        }

        //Number of tickets per status
        public int GetTicketsByStatusCount(TicketStatus status , int id = 0)
        {
            try
            {
                if (id != 0)
                {
                    return (from t in db.Tickets.Where(s => s.Status == status && s.ClientId == id || s.Status == status && s.EmployeeId == id) select t).Count();
                }
                else
                {
                    return (from t in db.Tickets.Where(s => s.Status == status) select t).Count();
                }

            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return 0;
            }
        }

        //Top 5 employees
        public IEnumerable<TopEmployeesView> GetTopEmployees()
        {
          
            try
            {
                var orderedEmployees = from u in db.Users
                                       join t in db.Tickets
                                       on u.Id equals t.EmployeeId
                                       where u.Type == UserType.Employee && t.Status == TicketStatus.Closed
                                       group u by u.Id into employeeGroups
                                       select new
                                       {
                                           Id = employeeGroups.Key,
                                           Count = employeeGroups.Count()
                                       };

                var topFiveEmployees = (from e in db.Users
                                        join o in orderedEmployees
                                        on e.Id equals o.Id
                                        orderby o.Count descending
                                        select new TopEmployeesView
                                        {
                                            FirstName = e.FirstName,
                                            LastName = e.LastName,
                                            Email = e.Email,
                                            Id = e.Id,
                                            TicketsTotal = o.Count
                                        }).Take(5);

                return topFiveEmployees;
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return null;
            }
        }

        //Number of tickets from users per country
        public int GetTicketsByUserCountryCount(UserCountry country)
        {
            try
            {
                return (from u in db.Users
                        join t in db.Tickets
                        on u.Id equals t.ClientId
                        where u.Type == UserType.Client && u.Country == country
                        select t).Count();
            }
            catch (Exception ex)
            {
                GlobalVariable.log.Write(LogLevel.Error, ex);
                return 0;
            }
        }
    }
}