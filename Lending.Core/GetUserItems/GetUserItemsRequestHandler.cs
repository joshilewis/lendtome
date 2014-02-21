using System;
using System.Linq;
using Lending.Core.Model;
using NHibernate;

namespace Lending.Core.GetUserItems
{
    public class GetUserItemsRequestHandler :  BaseAuthenticatedRequestHandler<GetUserItemsRequest, object>
    {
        public GetUserItemsRequestHandler(Func<ISession> sessionFunc)
             :base(sessionFunc)
        { }

        protected GetUserItemsRequestHandler() { }

        public override object HandleRequest(GetUserItemsRequest request, int userId)
        {
            Ownership<User>[] ownerships = Session
                .QueryOver<Ownership<User>>()
                .JoinQueryOver(x => x.Owner)
                .Where(x => x.Id == userId)
                .List()
                .ToArray()
                ;

            return ownerships
                .Select(x => new UserOwnership(x.Id, x.Item))
                .ToArray()
                ;


            return new GetUserItemsRequestResponse(ownerships);
        }
    }
}
