using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.Common.Models.TicketModels
{
    public class AddReplyView
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
    }


}
