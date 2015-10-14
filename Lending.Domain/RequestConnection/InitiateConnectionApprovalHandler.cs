using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Cqrs;
using Lending.Domain.Model;
using NHibernate;

namespace Lending.Domain.RequestConnection
{
    public class InitiateConnectionApprovalHandler : CommandHandler<InitiateConnectionApproval, Result>
    {
        public InitiateConnectionApprovalHandler(Func<ISession> getSession, Func<IRepository> getRepository,
            ICommandHandler<InitiateConnectionApproval, Result> nextHandler) : base(getSession, getRepository, nextHandler)
        {
        }

        public override Result HandleCommand(InitiateConnectionApproval command)
        {
            User targetUser = User.CreateFromHistory(Repository.GetEventsForAggregate<User>(command.AggregateId));
            Result result = targetUser.InitiateConnectionApproval(command.ProcessId, command.RequestingUserId);

            if (!result.Success) return result;

            Repository.Save(targetUser);

            if (NextHandler == null) return result;

            return NextHandler.HandleCommand(command);
        }

    }
}
