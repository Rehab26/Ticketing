using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TicketingSystem.Web.Client.Models
{
    public class ResetPassword
    {
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)\S{6,20}$", ErrorMessage = "The password must be at least 6 characters consist of digits lower and uppercase engilsh letters")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string Link { get; set; }
    }
}