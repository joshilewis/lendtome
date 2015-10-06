using System;
using Lending.Domain.Model;

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
            User user = User.CreateFromEvents(repository.GetEventsForAggregate<User>(request.FromUserId));
            user.RequestConnectionTo(request.ToUserId);
            repository.Save(user);
            return new BaseResponse();
        }

    }
}