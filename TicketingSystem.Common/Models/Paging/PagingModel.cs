using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.Common.Models.Paging
{
  public class PagingModel
    {
        public int DisplayLength { get; set; }
        public int DisplayStart { get; set; }
        public string SearchValue { get; set; } = null;
        public string FilterByStatus { get; set; } = null;
        public string FilterByPriority { get; set; } = null;
        public string FilterByCategory { get; set; } = null;
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
