using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Common.Enums;

namespace TicketingSystem.Common.Models.TicketModels
{
    public class AddFileStorageView
    {

        public string Id { get; set; }
        public AttachmentType Type { get; set; }
        public string Path { get; set; }
        public int? Reference { get; set; }

    }
}
