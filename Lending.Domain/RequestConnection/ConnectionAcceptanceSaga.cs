using Lending.Cqrs;

namespace Lending.Domain.RequestConnection
{
    public class ConnectionAcceptanceSaga : Lending.Cqrs.EventHandler<ConnectionRequested>
    {
        private readonly ICommandHandler<InitiateConnectionAcceptance, Result> commandHandler;

        public ConnectionAcceptanceSaga(ICommandHandler<InitiateConnectionAcceptance, Result> commandHandler)
        {
            this.commandHandler = commandHandler;
        }

        public override void When(ConnectionRequested @event)
        {
            commandHandler.HandleCommand(new InitiateConnectionAcceptance(@event.ProcessId, @event.TargetUserId, @event.AggregateId));
        }
    }
}