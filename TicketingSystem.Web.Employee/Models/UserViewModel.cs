using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TicketingSystem.Common.Enums;

namespace TicketingSystem.Web.Employee.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        [Display(Name ="First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Mobile")]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        [Display(Name ="Birthday")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public int TicketId { get; set; }
        [Display(Name = "Number of Tickets")]
        public int TicketsTotal { get; set; }
        public UserStatus UserStatus { get; set; }
        public UserType Type { get; set; }
        public string DateOfBirthFormatted { get { return this.DateOfBirth.ToString("dd/MM/yyyy"); } }

    }
}