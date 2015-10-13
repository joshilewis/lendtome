using System;
using Lending.Domain.Model;
using Lending.Domain.Persistence;
using NHibernate;

namespace Lending.Domain.RequestConnection
{
    public class RequestConnectionForSourceUser : AuthenticatedCommandHandler<RequestConnection, Response>
    {
        public const string TargetUserDoesNotExist = "The target user does not exist";
        public const string CantConnectToSelf = "You can't connect to yourself";

        public RequestConnectionForSourceUser(Func<ISession> getSession, Func<IRepository> getRepository,
            ICommandHandler<RequestConnection, Response> nextHandler) : base(getSession, getRepository, nextHandler)
        {
        }

        public override Response HandleCommand(RequestConnection command)
        {
            if (command.TargetUserId == command.AggregateId) return new Response(CantConnectToSelf);

            RegisteredUser targetUser = Session.Get<RegisteredUser>(command.TargetUserId);
            if (targetUser == null) return new Response(TargetUserDoesNotExist);

            User user = User.CreateFromHistory(Repository.GetEventsForAggregate<User>(command.AggregateId));

            Response  response = user.RequestConnectionTo(command.ProcessId, command.TargetUserId);

            if (!response.Success) return response;

            Repository.Save(user);

            return NextHandler.HandleCommand(command);
        }

    }
}