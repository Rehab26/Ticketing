using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Common.Enums;

namespace TicketingSystem.Common.Models.TicketModels
{
    public class ViewTicket
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public TicketStatus Status { get; set; }
        public TicketCategory Category { get; set; }
        public TicketPriority Priority { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public int ClosedBy { get; set; }
        public String ClosedByEmployee { get; set; }
        //public string Attachments { get; set; }
        public string[] Attachments { get; set; }
        public string[] Contents { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
    }
}
