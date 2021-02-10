using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.AvailoService
{
    public class AvailoResponseModel
    {
        public class Data {
            public int Status { get; set; }
            public int Score { get; set; }
            public string data { get; set; }
        }

        public bool Status { get; set; }
        public string Message { get; set; }

        public Data data { get; set; }
    }
}
