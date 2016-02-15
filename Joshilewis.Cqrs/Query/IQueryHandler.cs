namespace Joshilewis.Cqrs.Query
{
    public interface IQueryHandler<in TQuery> : IMessageHandler<TQuery> where TQuery : Query 
    {
    }
}