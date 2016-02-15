using Joshilewis.Cqrs.Query;

namespace Joshilewis.Cqrs.Command
{
    public interface ICommandHandler<in TCommand> : IMessageHandler<TCommand, EResultCode> where TCommand : Command
    {
    }
}
