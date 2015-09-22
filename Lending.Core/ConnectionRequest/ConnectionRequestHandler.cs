using System;

namespace Lending.Core.ConnectionRequest
{
    public class ConnectionRequestHandler : IRequestHandler<ConnectionRequest, BaseResponse>
    {
        public const string ConnectionAlreadyRequested = "A connection request for these users already exists";

        private readonly IEventEmitter eventEmitter;

        public ConnectionRequestHandler(IEventEmitter eventEmitter)
        {
            this.eventEmitter = eventEmitter;
        }

        public virtual BaseResponse HandleRequest(ConnectionRequest request)
        {
            eventEmitter.EmitEvent("User-"+request.FromUserId, new ConnectionRequested(Guid.NewGuid(), request.FromUserId, request.ToUserId));
            return new BaseResponse();
        }

    }
}