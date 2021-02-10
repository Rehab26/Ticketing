using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using TicketingSystem.Common.Enums;

namespace TicketingSystem.Web.Employee.Models
{
    public class UserLoginModel
    {
        public int Id { get; set; }
        public UserType Type { get; set; } = UserType.Employee;
        [DataType(DataType.PhoneNumber)]
        [Required]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^(009665|9665|\+9665|05|5)(5|0|3|6|4|9|1|8|7)([0-9]{7})$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string File { get; set; }
        public string Token { get; set; } // add for token

        public string FirstName { get; set; }

        public LoginApproach LoginType { get; set; }

    }
}