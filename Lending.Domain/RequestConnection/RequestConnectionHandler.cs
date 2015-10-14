using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace Lending.Domain.RequestConnection
{
    public class RequestConnectionHandler : AuthenticatedCommandHandler<RequestConnection, Result>
    {
        public RequestConnectionHandler(Func<ISession> getSession, Func<IRepository> getRepository)
            : base(getSession, getRepository, null)
        {
        }

        public override Result HandleCommand(RequestConnection command)
        {
            return new RequestConnectionForRequestingUser(() => Session, () => Repository,
                new RequestConnectionForTargetUser(() => Session, () => Repository, null))
                .HandleCommand(command);
        }
    }
}
