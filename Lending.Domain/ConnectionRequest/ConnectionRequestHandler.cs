using System;

namespace Lending.Domain.ConnectionRequest
{
    public class ConnectionRequestHandler : IRequestHandler<ConnectionRequest, BaseResponse>
    {
        public const string ConnectionAlreadyRequested = "A connection request for these users already exists";

        private readonly IRepository repository;

        public ConnectionRequestHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public virtual BaseResponse HandleRequest(ConnectionRequest request)
        {
            //repository.EmitEvent("User-"+request.FromUserId, new ConnectionRequested(Guid.NewGuid(), request.FromUserId, request.ToUserId));
            return new BaseResponse();
        }

    }
}