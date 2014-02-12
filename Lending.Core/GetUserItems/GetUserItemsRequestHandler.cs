using System;
using System.Linq;
using Lending.Core.Model;
using NHibernate;

namespace Lending.Core.GetUserItems
{
    public class GetUserItemsRequestHandler : IRequestHandler<GetUserItemsRequest, GetUserItemsRequestResponse>
    {
        private readonly Func<ISession> getSession;

        public GetUserItemsRequestHandler(Func<ISession> sessionFunc)
        {
            this.getSession = sessionFunc;
        }

        protected GetUserItemsRequestHandler() { }


        public GetUserItemsRequestResponse HandleRequest(GetUserItemsRequest request)
        {
            ISession session = getSession();

            Ownership[] ownerships = session
                .QueryOver<Ownership<User>>()
                .JoinQueryOver(x => x.Owner)
                .Where(x => x.Id == request.UserId)
                .List<Ownership>()
                .ToArray()
                ;

            return new GetUserItemsRequestResponse(ownerships);
        }
    }
}
