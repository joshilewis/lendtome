using Lending.Cqrs;

namespace Lending.Domain.RequestConnection
{
    public class ConnectionApprovalSaga : Lending.Cqrs.EventHandler<ConnectionRequested>
    {
        private readonly ICommandHandler<InitiateConnectionApproval, Result> commandHandler;

        public ConnectionApprovalSaga(ICommandHandler<InitiateConnectionApproval, Result> commandHandler)
        {
            this.commandHandler = commandHandler;
        }

        public override void When(ConnectionRequested @event)
        {
            commandHandler.HandleCommand(new InitiateConnectionApproval(@event.ProcessId, @event.TargetUserId, @event.AggregateId));
        }
    }
}