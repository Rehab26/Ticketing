using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TicketingSystem.Common.Models.UserModels
{
    public class PasswordForget
    {
        public string Email { get; set; }

        public string Link { get; set; }

    }
}