using System;
using Lending.Cqrs;
using Lending.Domain.Model;
using Lending.Domain.Persistence;
using NHibernate;

namespace Lending.Domain.AddBookToCollection
{
    public class AddBookToCollectionHandler : AuthenticatedCommandHandler<AddBookToCollection, Result>
    {
        private readonly Func<Guid> getNextGuid;

        public AddBookToCollectionHandler(Func<ISession> sessionFunc, Func<IRepository> repositoryFunc, Func<Guid> nextGuidFunc)
            : base(sessionFunc, repositoryFunc)
        {
            getNextGuid = nextGuidFunc;
        }

        public override Result HandleCommand(AddBookToCollection command)
        {
            AddedBook addedBook = Session.QueryOver<AddedBook>()
                .Where(x => x.Title == command.Title)
                .Where(x => x.Author == command.Author)
                .Where(x => x.Isbn == command.Isbn)
                .SingleOrDefault();

            if (addedBook == null)
            {
                Guid newBookId = getNextGuid();
                Book book = Book.AddBook(command.ProcessId, newBookId, command.Title, command.Author, command.Isbn);
                addedBook = new AddedBook(newBookId, command.Title, command.Author, command.Isbn);
                Session.Save(addedBook);
                Repository.Save(book);
            }

            User user = User.CreateFromHistory(Repository.GetEventsForAggregate<User>(command.AggregateId));
            Result result = user.AddBookToCollection(command.ProcessId, addedBook.Id);
            Repository.Save(user);

            return Success();
        }
    }
}