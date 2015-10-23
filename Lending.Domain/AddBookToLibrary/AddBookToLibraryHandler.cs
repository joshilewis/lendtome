using System;
using Lending.Cqrs;
using Lending.Domain.Model;
using NHibernate;

namespace Lending.Domain.AddBookToLibrary
{
    public class AddBookToLibraryHandler : AuthenticatedCommandHandler<AddBookToLibrary, Result>
    {
 
        public AddBookToLibraryHandler(Func<ISession> sessionFunc, Func<IRepository> repositoryFunc)
            : base(sessionFunc, repositoryFunc)
        {
        }

        public override Result HandleCommand(AddBookToLibrary command)
        {

            User user = User.CreateFromHistory(Repository.GetEventsForAggregate<User>(command.AggregateId));
            Result result = user.AddBookToLibrary(command.ProcessId, command.Title, command.Author, command.Isbn);
            if (!result.Success) return result;

            Repository.Save(user);

            return Success();
        }
    }
}