using System;
using System.ComponentModel.DataAnnotations;
using TicketingSystem.Common.Enums;

namespace TicketingSystem.Web.Client.Models
{
    public class TicketSave
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        [Display(Name = "Issue title")]
        [Required]
        [StringLength(90, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
        public string Title { get; set; }
        [Display(Name = "Product")]
        [Required]
        public TicketCategory Category { get; set; }
        [Display(Name = "Ticket Priority")]
        [Required]
        public TicketPriority Priority { get; set; }
        public DateTime OpenDate { get; set; }
        public TicketStatus Status { get; set; }
        [Display(Name = "Issue Description")]
        [DataType(DataType.MultilineText)]
        [Required]
        [StringLength(5000, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 20)]
        public string Description { get; set; }
        [FileExtensions(Extensions = "jpg,png,gif,jpeg,bmp", ErrorMessage = "Please only upload image files")]
        public string[] Attachments { get; set; }
        
    }
}