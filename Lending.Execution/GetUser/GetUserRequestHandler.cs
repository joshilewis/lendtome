using System;
using Lending.Core;
using Lending.Execution.Auth;
using NHibernate;

namespace Lending.Execution.GetUser
{
    public class GetUserRequestHandler : IRequestHandler<GetUserRequest, GetUserRequestResponse>
    {
         private readonly Func<ISession> getSession;

        public GetUserRequestHandler(Func<ISession> sessionFunc)
        {
            this.getSession = sessionFunc;
        }

        protected GetUserRequestHandler() { }

        public GetUserRequestResponse HandleRequest(GetUserRequest request)
        {
            Guid userId = getSession()
                .QueryOver<ServiceStackUser>()
                .JoinQueryOver(x => x.AuthenticatedUser)
                .Where(x => x.Id == request.UserAuthId)
                .SingleOrDefault<ServiceStackUser>()
                .Id;

            return new GetUserRequestResponse(userId);
        }
    }
}
