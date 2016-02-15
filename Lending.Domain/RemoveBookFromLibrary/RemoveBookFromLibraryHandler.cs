using System;
using Joshilewis.Cqrs;
using Joshilewis.Cqrs.Command;
using Joshilewis.Cqrs.Query;
using Lending.Domain.Model;

namespace Lending.Domain.RemoveBookFromLibrary
{
    public class RemoveBookFromLibraryHandler : AuthenticatedCommandHandler<RemoveBookFromLibrary>
    {
        public RemoveBookFromLibraryHandler(Func<IEventRepository> eventRepositoryFunc)
            : base(eventRepositoryFunc)
        {
        }

        public override EResultCode Handle(RemoveBookFromLibrary command)
        {
            Library library = Library.CreateFromHistory(EventRepository.GetEventsForAggregate<Library>(command.AggregateId));
            library.CheckUserAuthorized(command.UserId);
            library.RemoveBookFromLibrary(command.ProcessId, command.Title, command.Author, command.Isbn);

            EventRepository.Save(library);
            return Success();
        }
    }
}