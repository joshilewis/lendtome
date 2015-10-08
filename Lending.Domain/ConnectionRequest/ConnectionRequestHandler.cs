using System;
using Lending.Domain.Model;

namespace Lending.Domain.ConnectionRequest
{
    public class ConnectionRequestHandler : IAuthenticatedRequestHandler<ConnectionRequest, BaseResponse>
    {
        public const string ConnectionAlreadyRequested = "A connection request for these users already exists";
        public const string TargetUserDoesNotExist = "The target user does not exist";

        private readonly IRepository repository;

        public ConnectionRequestHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public virtual BaseResponse HandleRequest(ConnectionRequest request, Guid userId)
        {
            User user = User.CreateFromEvents(repository.GetEventsForAggregate<User>(userId));
            bool connectionRequestSuccessful = user.RequestConnectionTo(request.TargetUserId);

            if (!connectionRequestSuccessful)
                return new BaseResponse(ConnectionAlreadyRequested);

            repository.Save(user);
            return new BaseResponse();
        }

    }
}