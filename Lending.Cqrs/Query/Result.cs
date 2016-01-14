namespace Lending.Cqrs.Query
{
    public class Result
    {
        public EResultCode Code { get; set; }

        public Result(EResultCode code)
        {
            Code = code;
        }

        public enum EResultCode : byte
        {
            Ok = 200,
            Created = 201,
        }
    }

    public class Result<T> : Result
    {
        public virtual T Payload { get; protected set; }

        public Result(T payload)
            : base(EResultCode.Ok)
        {
            Payload = payload;
        }
        public Result(EResultCode code, T payload)
            : base(code)
        {
            Payload = payload;
        }
    }
}
