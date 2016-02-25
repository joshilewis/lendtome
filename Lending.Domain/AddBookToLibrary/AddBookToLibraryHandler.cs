using System;
using Joshilewis.Cqrs;
using Joshilewis.Cqrs.Command;
using Joshilewis.Cqrs.Query;
using Lending.Domain.Model;

namespace Lending.Domain.AddBookToLibrary
{
    public class AddBookToLibraryHandler : AuthenticatedCommandHandler<AddBookToLibrary>
    {
 
        public AddBookToLibraryHandler(Func<IEventRepository> repositoryFuncFunc)
            : base(repositoryFuncFunc)
        {
        }

        public override object Handle(AddBookToLibrary command)
        {
            Library library = Library.CreateFromHistory(EventRepository.GetEventsForAggregate<Library>(command.AggregateId));
            library.CheckUserAuthorized(command.UserId);
            library.AddBookToLibrary(command.ProcessId, command.Title, command.Author, command.Isbn, command.PublishDate);

            EventRepository.Save(library);

            return Created();
        }
    }
}