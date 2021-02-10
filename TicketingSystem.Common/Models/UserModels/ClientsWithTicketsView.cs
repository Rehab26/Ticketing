using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Common.Enums;

namespace TicketingSystem.Common.Models.UserModels
{
    public class ClientsWithTicketsView
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public UserStatus UserStatus { get; set; }
        public int TicketsTotal { get; set; }
    }
}
