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
    public class InitiateConnectionAcceptanceHandler : CommandHandler<InitiateConnectionAcceptance, Result>
    {
        public InitiateConnectionAcceptanceHandler(Func<ISession> getSession, Func<IRepository> getRepository) 
            : base(getSession, getRepository)
        {
        }

        public override Result HandleCommand(InitiateConnectionAcceptance command)
        {
            User targetUser = User.CreateFromHistory(Repository.GetEventsForAggregate<User>(command.AggregateId));
            Result result = targetUser.InitiateConnectionAcceptance(command.ProcessId, command.RequestingUserId);

            if (!result.Success) return result;

            Repository.Save(targetUser);

            return result;
        }

    }
}
