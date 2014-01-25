using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class BaseResponse
    {
        public bool Success { get; set; }
        public string FailureDescription { get; set; }

        public BaseResponse()
        {
            Success = true;
            FailureDescription = null;
        }

        public BaseResponse(string failureDescription)
        {
            FailureDescription = failureDescription;
        }
    }

    public class BaseResponse<T> : BaseResponse
    {
        public T Payload { get; set; }

        public BaseResponse(T payload)
            : base()
        {
            Payload = payload;
        }

        public BaseResponse(string failureDescription, T payload)
            : base(failureDescription)
        {
            Payload = payload;
        }
    }
}
