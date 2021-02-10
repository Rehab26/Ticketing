using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketingSystem.Web.Employee.Models
{
    public class TicketAssignment
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public int Id { get; set; }

    }
}