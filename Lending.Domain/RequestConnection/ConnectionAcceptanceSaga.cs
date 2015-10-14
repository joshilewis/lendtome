using Lending.Cqrs;

namespace Lending.Domain.RequestConnection
{
    public class ConnectionAcceptanceSaga : EventHandler<ConnectionRequested>
    {
        private readonly ICommandHandler<InitiateConnectionAcceptance, Result> initiateAcceptanceCommandHandler;

        public ConnectionAcceptanceSaga(ICommandHandler<InitiateConnectionAcceptance, Result> initiateAcceptanceCommandHandler)
        {
            this.initiateAcceptanceCommandHandler = initiateAcceptanceCommandHandler;
        }

        public override void When(ConnectionRequested @event)
        {
            initiateAcceptanceCommandHandler.HandleCommand(new InitiateConnectionAcceptance(@event.ProcessId, @event.TargetUserId, @event.AggregateId));
        }


    }
}