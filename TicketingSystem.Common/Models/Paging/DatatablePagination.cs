using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketingSystem.Common.Models.Paging
{
    public class DatatablePagination<T>
    {
        public List<T> Data { get; set; }
        public int TotalRecord { get; set; }
        public int TotalFilteredRecord { get; set; }

    }
}
