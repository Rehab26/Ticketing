using System.ComponentModel.DataAnnotations;
using TicketingSystem.Common.Enums;
using TicketingSystem.Common.Models.TicketModels;

namespace TicketingSystem.Web.Employee.Models
{
    public class TicketView
    {
        public int Id { get; set; }
        public string Client { get; set; }
        public TicketStatus Status { get; set; }
        public TicketCategory Category { get; set; }
        public TicketPriority Priority { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [Display(Name = "Open Date")]
        public string OpenDate { get; set; }
        public string ClosedDate { get; set; }
        public string ClosedBy { get; set; }
        public string[] Attachments { get; set; }
        public ReplyModel[] Comments { get; set; }

    }
}