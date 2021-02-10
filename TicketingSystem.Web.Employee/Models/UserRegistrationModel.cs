using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TicketingSystem.Common.Enums;
using TicketingSystem.Common.Models;

namespace TicketingSystem.Web.Employee.Models
{
    public class UserRegistrationModel
    {
        public int Id { get; set; }

        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        [Required]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please. Name without spaces")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please. Name without spaces")]
        public string LastName { get; set; }

        public UserType Type { get; set; } = UserType.Client;

        [Display(Name = "Date of Birth")]
        [DateMinimumAge(18, ErrorMessage = "{0} must be someone at least {1} years of age")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^(009665|9665|\+9665|05|5)(5|0|3|6|4|9|1|8|7)([0-9]{7})$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public LoginApproach LoginType { get; set; }

        public string File { get; set; }

        public UserCountry Country { get; set; }

    }
}
