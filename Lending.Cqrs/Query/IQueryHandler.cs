using Lending.Cqrs.Command;

namespace Lending.Cqrs.Query
{
    public interface IQueryHandler<TQuery, TResultPayload>
    {
        Result<TResultPayload> HandleQuery(TQuery query);
    }
}