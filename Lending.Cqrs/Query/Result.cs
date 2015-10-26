namespace Lending.Cqrs.Query
{
    public class Result
    {
        public bool Success { get; set; }
        public string Reason { get; set; }

        public Result()
        {
            Success = true;
            Reason = null;
        }

        public Result(string reason)
        {
            Reason = reason;
            Success = false;
        }
    }

    public class Result<T> : Result
    {
        public virtual T Payload { get; protected set; }

        public Result(T payload)
            : base()
        {
            Payload = payload;
        }

        public Result(string reason, T payload)
            : base(reason)
        {
            Payload = payload;
        }
    }
}
