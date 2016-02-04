namespace Lending.Cqrs.Query
{
    public class Result
    {
        public EResultCode Code { get; set; }

        public Result(EResultCode code)
        {
            Code = code;
        }

        public enum EResultCode
        {
            Ok = 200,
            Created = 201,
        }
    }

    public class Result<TPayload> : Result
    {
        public virtual TPayload Payload { get; protected set; }

        public Result(TPayload payload)
            : base(EResultCode.Ok)
        {
            Payload = payload;
        }
        public Result(EResultCode code, TPayload payload)
            : base(code)
        {
            Payload = payload;
        }

        public Result()
            : base(EResultCode.Ok)
        {
            Payload = default(TPayload);
        }

    }
}
