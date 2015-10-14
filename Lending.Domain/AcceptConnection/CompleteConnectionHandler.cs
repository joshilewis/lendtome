using System;
using Lending.Cqrs;
using Lending.Domain.Model;
using NHibernate;

namespace Lending.Domain.AcceptConnection
{
    public class CompleteConnectionHandler : CommandHandler<CompleteConnection, Result>
    {
        public CompleteConnectionHandler(Func<ISession> getSession, Func<IRepository> getRepository)
            : base(getSession, getRepository)
        {
        }

        public override Result HandleCommand(CompleteConnection command)
        {
            User user = User.CreateFromHistory(Repository.GetEventsForAggregate<User>(command.AggregateId));

            Result result = user.CompleteConnection(command.ProcessId, command.AcceptingUserId);

            if (!result.Success) return result;

            Repository.Save(user);

            return result;
        }
    }
}