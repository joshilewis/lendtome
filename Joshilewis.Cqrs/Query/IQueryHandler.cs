namespace Joshilewis.Cqrs.Query
{
    public interface IQueryHandler<in TQuery, out TResult> : IMessageHandler<TQuery, TResult> where TQuery : Query 
    {
    }
}