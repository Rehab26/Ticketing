using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Common.Enums;
using TicketingSystem.Common.Models.UserModels;

namespace TicketingSystem.Common.Models.TicketModels
{
    public class TicketSave
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public TicketStatus Status { get; set; }
        public TicketCategory Category { get; set; }
        public TicketPriority Priority { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public int? ClosedBy { get; set; }
        public string[] Attachments { get; set; }
        public int? EmployeeId { get; set; }

    }
}
