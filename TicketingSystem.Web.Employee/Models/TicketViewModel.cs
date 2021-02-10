using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TicketingSystem.Common.Enums;

namespace TicketingSystem.Web.Employee.Models
{
    public class TicketViewModel
    {
        public int Id { get; set; }
        public TicketStatus Status { get; set; }
        public TicketCategory Category { get; set; }
        public TicketPriority Priority { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [Display(Name="Open Date")]
        public DateTime OpenDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public int? ClosedBy { get; set; }
        public int EmployeeId { get; set; }
        [Display(Name = "Assigned to")]
        public string EmployeeName { get; set; }
        [Display(Name = "Client")]
        public string ClientName { get; set; }

        public string OpenDateFormatted { get { return this.OpenDate.ToString("dddd, dd MMMM yyyy hh:mm tt"); } }

    }
}