using System;
using Lending.Cqrs;
using Lending.Domain.Model;
using Lending.Domain.Persistence;
using NHibernate;

namespace Lending.Domain.AddBookToCollection
{
    public class AddBookToCollectionHandler : AuthenticatedCommandHandler<AddBookToCollection, Result>
    {
 
        public AddBookToCollectionHandler(Func<ISession> sessionFunc, Func<IRepository> repositoryFunc)
            : base(sessionFunc, repositoryFunc)
        {
        }

        public override Result HandleCommand(AddBookToCollection command)
        {

            User user = User.CreateFromHistory(Repository.GetEventsForAggregate<User>(command.AggregateId));
            Result result = user.AddBookToLibrary(command.ProcessId, command.Title, command.Author, command.Isbn);
            if (!result.Success) return result;

            Repository.Save(user);

            return Success();
        }
    }
}