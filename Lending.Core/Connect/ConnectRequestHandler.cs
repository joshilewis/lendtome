using System;
using Lending.Core.Model;
using NHibernate;

namespace Lending.Core.Connect
{
    public class ConnectRequestHandler : IRequestHandler<ConnectRequest, BaseResponse>
    {
        public const string ConnectionAlreadyRequested = "A connection request for these users already exists";

        private readonly IEventEmitter eventEmitter;

        public ConnectRequestHandler(IEventEmitter eventEmitter)
        {
            this.eventEmitter = eventEmitter;
        }

        public virtual BaseResponse HandleRequest(ConnectRequest request)
        {
            eventEmitter.EmitEvent("User-"+request.FromUserId, new ConnectionRequested(Guid.NewGuid(), request.FromUserId, request.ToUserId));
            return new BaseResponse();
        }

    }
}