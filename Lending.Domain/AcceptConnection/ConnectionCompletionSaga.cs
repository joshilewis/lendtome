using Lending.Cqrs;

namespace Lending.Domain.AcceptConnection
{
    public class ConnectionCompletionSaga : EventHandler<ConnectionAccepted>

    {
        private readonly ICommandHandler<CompleteConnection, Result> completeConnectionCommandHandler;

        public ConnectionCompletionSaga(ICommandHandler<CompleteConnection, Result> completeConnectionCommandHandler)
        {
            this.completeConnectionCommandHandler = completeConnectionCommandHandler;
        }

        public override void When(ConnectionAccepted @event)
        {
            completeConnectionCommandHandler.HandleCommand(new CompleteConnection(@event.ProcessId,
                @event.RequestingUserId, @event.AggregateId));
        }
    }
}
