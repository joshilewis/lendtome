using System;
using System.Linq;
using Lending.Core.Model;
using NHibernate;

namespace Lending.Core.GetUserItems
{
    public class GetUserItemsRequestHandler :  BaseAuthenticatedRequestHandler<GetUserItemsRequest, GetUserItemsRequestResponse>
    {
        public GetUserItemsRequestHandler(Func<ISession> sessionFunc)
             :base(sessionFunc)
        { }

        protected GetUserItemsRequestHandler() { }

        public override GetUserItemsRequestResponse HandleRequest(GetUserItemsRequest request, int userId)
        {
            Ownership<User>[] ownerships = Session
                .QueryOver<Ownership<User>>()
                .JoinQueryOver(x => x.Owner)
                .Where(x => x.Id == userId)
                .List()
                .ToArray()
                ;

            return new GetUserItemsRequestResponse(ownerships);
        }
    }
}
