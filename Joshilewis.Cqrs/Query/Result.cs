namespace Joshilewis.Cqrs.Query
{
    public class Result
    {
        public EResultCode Code { get; set; }

        public Result(EResultCode code)
        {
            Code = code;
        }
    }

    public enum EResultCode
    {
        Ok = 200,
        Created = 201,
    }
}
