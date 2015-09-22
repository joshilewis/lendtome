using System.Runtime.Serialization;

namespace Lending.Core
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
            Success = false;
        }
    }

    public class BaseResponse<T> : BaseResponse
    {
        public virtual T Payload { get; protected set; }

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
