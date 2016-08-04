using Joshilewis.Cqrs.Query;

namespace Joshilewis.Cqrs.Command
{
    public interface IAuthenticatedCommandHandler<in TCommand> : ICommandHandler<TCommand>,
        IAuthenticatedMessageHandler<TCommand> 
        where TCommand : AuthenticatedCommand
    {
    }
}
