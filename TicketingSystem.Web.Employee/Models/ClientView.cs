using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicketingSystem.Common.Enums;

namespace TicketingSystem.Web.Employee.Models
{
    public class ClientView
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string DateOfBirth { get; set; }
        public UserCountry Country { get; set; }
        public UserType Type { get; set; }
        public string Token { get; set; } //added for token
    }
}