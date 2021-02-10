using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TicketingSystem.Common.Enums;
using TicketingSystem.Common.Models;

namespace TicketingSystem.Web.Client.Models
{
    public class ClientSaveModel
    {
        public int Id { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        [Required]
        public string Email { get; set; }

        [Required(ErrorMessage = "The First Name is Required")]
        [Display(Name = "First Name")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please. Name without spaces")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The Last Name is Required")]
        [Display(Name = "Last Name")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please. Name without spaces")]
        public string LastName { get; set; }
       
        [DataType(DataType.Date)]
        [DateMinimumAge(18, ErrorMessage = "{0} must be someone at least {1} years of age")]
        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "The Phone Number is Required")]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^(009665|9665|\+9665|05|5)(5|0|3|6|4|9|1|8|7)([0-9]{7})$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)\S{6,20}$", ErrorMessage = "The password must be at least 6 characters consist of digits lower and uppercase engilsh letters")]
        public string Password { get; set; }

        public UserCountry Country { get; set; }

        public AuthenticationModel Authentication { get; set; }
    }
}