using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSystem.Common.Enums;

namespace TicketingSystem.Common.Models.Paging
{
    public class TicktingsystemResponse
    {
        public ErrorMessage ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public int RowsCount { get; set; }


        public TicktingsystemResponse(ErrorMessage errorCode = Enums.ErrorMessage.Success, string errorMessage = "")
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            RowsCount = 0;
        }

    }


    public class TicktingsystemResponse<T> : TicktingsystemResponse
    {
        public T Data { get; set; }

        public TicktingsystemResponse(T data = default(T), ErrorMessage errorCode = Enums.ErrorMessage.Success, string errorMessage = "")
            : base(errorCode, errorMessage)
        {
            Data = data;
        }
    }
}

