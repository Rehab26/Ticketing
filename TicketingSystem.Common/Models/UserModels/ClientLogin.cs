using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.Common.Models.UserModels
{
    public class ClientLogin
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Token { get; set; } 
    }
}

