using System;
using Joshilewis.Cqrs;
using Joshilewis.Cqrs.Command;
using Joshilewis.Cqrs.Query;
using Lending.Domain.Model;

namespace Lending.Domain.RemoveBookFromLibrary
{
    public class BookRemover : AuthenticatedCommandHandler<RemoveBookFromLibrary>
    {
        public BookRemover(Func<IEventRepository> eventRepositoryFunc)
            : base(eventRepositoryFunc)
        {
        }

        public override object Handle(RemoveBookFromLibrary command)
        {
            Library library = Library.CreateFromHistory(EventRepository.GetEventsForAggregate<Library>(command.AggregateId));
            library.CheckUserAuthorized(new AdministratorId(command.UserId));
            library.RemoveBookFromLibrary(command.ProcessId, command.Title, command.Author, command.Isbn, command.PublishYear, command.CoverPicture);

            EventRepository.Save(library);
            return Success();
        }
    }
}