using Lending.Cqrs.Query;

namespace Lending.Cqrs.Command
{
    public interface IAuthenticatedCommandHandler<in TCommand, out TResult, TAggregate> :
        ICommandHandler<TCommand, TResult, TAggregate>, IAuthenticatedMessageHandler<TCommand, TResult>
        where TCommand : AuthenticatedCommand where TResult : Result where TAggregate : Aggregate
    {
    }
}
