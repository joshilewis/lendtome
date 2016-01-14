using Lending.Cqrs.Query;

namespace Lending.Cqrs.Command
{
    public interface ICommandHandler<in TCommand, out TResult, TAggregate> : IMessageHandler<TCommand, TResult>
        where TCommand : Command where TResult : Result where TAggregate : Aggregate
    {
    }
}
