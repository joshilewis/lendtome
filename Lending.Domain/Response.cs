namespace Lending.Domain
{
    public class Response
    {
        public bool Success { get; set; }
        public string FailureDescription { get; set; }

        public Response()
        {
            Success = true;
            FailureDescription = null;
        }

        public Response(string failureDescription)
        {
            FailureDescription = failureDescription;
            Success = false;
        }
    }

    public class Response<T> : Response
    {
        public virtual T Payload { get; protected set; }

        public Response(T payload)
            : base()
        {
            Payload = payload;
        }

        public Response(string failureDescription, T payload)
            : base(failureDescription)
        {
            Payload = payload;
        }
    }
}
