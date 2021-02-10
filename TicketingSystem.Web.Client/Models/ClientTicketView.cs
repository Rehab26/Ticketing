using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketingSystem.Web.Client.Models
{
    public class ClientTicketView
    {
        public IList<TicketingSystem.Web.Client.Models.TicketSave> Tickets { get; set; }
        public ClientView Client { get; set; }
    }
}