using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Domain.Model;
using NHibernate;

namespace Lending.Domain.RequestConnection
{
    public class RequestConnectionForTargetUser : AuthenticatedCommandHandler<RequestConnection, Result>
    {
        public RequestConnectionForTargetUser(Func<ISession> getSession, Func<IRepository> getRepository, ICommandHandler<RequestConnection, Result> nextHandler) : base(getSession, getRepository, nextHandler)
        {
        }

        public override Result HandleCommand(RequestConnection command)
        {
            User targetUser = User.CreateFromHistory(Repository.GetEventsForAggregate<User>(command.TargetUserId));
            Result result = targetUser.ReceiveConnectionRequest(command.ProcessId, command.AggregateId);

            if (!result.Success) return result;

            Repository.Save(targetUser);

            if (NextHandler == null) return result;

            return NextHandler.HandleCommand(command);
        }

    }
}
