using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicketingSystem.Common.Enums;

namespace TicketingSystem.Web.Employee.Models
{
    public class TicketSaveModel
    {
        public int Id { get; set; }
        public TicketStatus Status { get; set; }
        public int? ClosedBy { get; set; }
        public int EmployeeId { get; set; }
    }
}