using Lending.Cqrs.Command;

namespace Lending.Cqrs.Query
{
    public interface IQueryHandler<in TQuery, out TResult> : IMessageHandler<TQuery, TResult> where TQuery : Query
        where TResult : Result
    {
    }
}