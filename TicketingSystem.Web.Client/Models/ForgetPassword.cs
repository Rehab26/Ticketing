using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TicketingSystem.Web.Client.Models
{
    public class ForgetPassword
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        public string Email { get; set; }

        public string Link { get; set; }

    }
}