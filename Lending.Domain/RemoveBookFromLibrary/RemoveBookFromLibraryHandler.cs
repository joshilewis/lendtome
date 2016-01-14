using System;
using Lending.Cqrs;
using Lending.Cqrs.Command;
using Lending.Cqrs.Query;
using Lending.Domain.Model;

namespace Lending.Domain.RemoveBookFromLibrary
{
    public class RemoveBookFromLibraryHandler : AuthenticatedCommandHandler<RemoveBookFromLibrary, Result>
    {
        public RemoveBookFromLibraryHandler(Func<IRepository> repositoryFunc, Func<IEventRepository> eventRepositoryFunc)
            : base(repositoryFunc, eventRepositoryFunc)
        {
        }

        public override Result Handle(RemoveBookFromLibrary command)
        {
            Library library = Library.CreateFromHistory(EventRepository.GetEventsForAggregate<Library>(command.AggregateId));
            Result result = library.RemoveBookFromLibrary(command.ProcessId, command.Title, command.Author, command.Isbn);

            EventRepository.Save(library);
            return Success();
        }
    }
}