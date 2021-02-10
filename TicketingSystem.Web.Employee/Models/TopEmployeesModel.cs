using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketingSystem.Web.Employee.Models
{
    public class TopEmployeesModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TicketsTotal { get; set; }
    }
}