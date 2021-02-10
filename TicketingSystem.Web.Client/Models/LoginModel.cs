using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TicketingSystem.Web.Client.Models
{
    public class LoginModel
    {

        public int Id { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^(009665|9665|\+9665|05|5)(5|0|3|6|4|9|1|8|7)([0-9]{7})$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}