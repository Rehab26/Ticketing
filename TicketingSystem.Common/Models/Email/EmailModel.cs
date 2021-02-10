using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.Common.Models.Email
{
    public class EmailModel
    {
        public string toname { get; set; }

        public string toemail { get; set; }

        public string subject { get; set; }

        public string message { get; set; }
    }
}
