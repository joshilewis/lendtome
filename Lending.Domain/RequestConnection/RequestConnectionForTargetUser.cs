using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Domain.Model;
using NHibernate;

namespace Lending.Domain.RequestConnection
{
    public class RequestConnectionForTargetUser : AuthenticatedCommandHandler<RequestConnection, Response>
    {
        public RequestConnectionForTargetUser(Func<ISession> getSession, Func<IRepository> getRepository, ICommandHandler<RequestConnection, Response> nextHandler) : base(getSession, getRepository, nextHandler)
        {
        }

        public override Response HandleCommand(RequestConnection command)
        {
            User targetUser = User.CreateFromHistory(Repository.GetEventsForAggregate<User>(command.TargetUserId));
            Response response = targetUser.ReceiveConnectionRequest(command.ProcessId, command.AggregateId);

            if (!response.Success) return response;

            Repository.Save(targetUser);

            return NextHandler.HandleCommand(command);
        }

    }
}
