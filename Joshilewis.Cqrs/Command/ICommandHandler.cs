using Joshilewis.Cqrs.Query;

namespace Joshilewis.Cqrs.Command
{
    public interface ICommandHandler<in TCommand> : IMessageHandler<TCommand> where TCommand : Command
    {
    }
}
