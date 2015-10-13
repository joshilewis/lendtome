using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Domain.AcceptConnection;
using NHibernate;

namespace Lending.Domain.AcceptConnection
{
    public class AcceptConnectionHandler : AuthenticatedCommandHandler<AcceptConnection, Response>
    {
        public AcceptConnectionHandler(Func<ISession> getSession, Func<IRepository> getRepository)
            : base(getSession, getRepository, null)
        {
        }

        public override Response HandleCommand(AcceptConnection command)
        {
            return new AcceptConnectionForAcceptingUser(() => Session, () => Repository,
                new AcceptConnectionForRequestingUser(() => Session, () => Repository, null))
                .HandleCommand(command);
        }
    }
}
