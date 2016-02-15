using Joshilewis.Cqrs.Query;

namespace Joshilewis.Cqrs.Command
{
    public interface IAuthenticatedCommandHandler<in TCommand, out TResult> : ICommandHandler<TCommand, TResult>,
        IAuthenticatedMessageHandler<TCommand, TResult> 
        where TCommand : AuthenticatedCommand
    {
    }
}
