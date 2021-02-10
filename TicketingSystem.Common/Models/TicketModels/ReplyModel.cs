using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Common.Models.UserModels;

namespace TicketingSystem.Common.Models.TicketModels
{
    public class ReplyModel
    {

        public int Id { get; set; }
        public int UserId { get; set; }
        public int TicketId { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public string Attachment { get; set; }
        public ViewUser User { get; set; }
    }


}
