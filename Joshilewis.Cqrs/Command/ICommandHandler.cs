using Joshilewis.Cqrs.Query;

namespace Joshilewis.Cqrs.Command
{
    public interface ICommandHandler<in TCommand, out TResult> : IMessageHandler<TCommand, TResult> where TCommand : Command where TResult : Result
    {
    }
}
