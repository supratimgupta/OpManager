using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class StatusDTO
    {
        public bool IsSuccess { get; set; }

        public string FailureReason { get; set; }

        public bool IsException { get; set; }

        public string ExceptionMessage { get; set; }

        public string HResult { get; set; }

        public string StackTrace { get; set; }
    }

    public class StatusDTO<T>:StatusDTO
    {
        public T ReturnObj { get; set; }
    }
}
