using System;
using Lending.Cqrs;
using Lending.Domain.Model;
using NHibernate;

namespace Lending.Domain.RemoveBookFromLibrary
{
    public class RemoveBookFromLibraryHandler : AuthenticatedCommandHandler<RemoveBookFromLibrary, Result>
    {
        public RemoveBookFromLibraryHandler(Func<ISession> getSession, Func<IRepository> getRepository)
            : base(getSession, getRepository)
        {
        }

        public override Result HandleCommand(RemoveBookFromLibrary command)
        {
            User user = User.CreateFromHistory(Repository.GetEventsForAggregate<User>(command.AggregateId));
            Result result = user.RemoveBookFromLibrary(command.ProcessId, command.Title, command.Author, command.Isbn);

            if (!result.Success) return result;

            Repository.Save(user);
            return Success();
        }
    }
}