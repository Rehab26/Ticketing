using System;
using TicketingSystem.Common.Enums;

namespace TicketingSystem.Common.Models.UserModels
{
    public class ViewUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserType Type { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public LoginApproach LoginType { get; set; }
        public UserStatus Status { get; set; }
        public string Password { get; set; }
        public int EmployeeId { get; set; }
        public string File { get; set; }
        public UserCountry Country { get; set; }
        public string Token { get; set; }
    }
}
