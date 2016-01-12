using System;
using Lending.Cqrs;
using Lending.Cqrs.Command;
using Lending.Cqrs.Query;
using Lending.Domain.Model;

namespace Lending.Domain.AddBookToLibrary
{
    public class AddBookToLibraryHandler : AuthenticatedCommandHandler<AddBookToLibrary, Result>
    {
 
        public AddBookToLibraryHandler(Func<IRepository> sessionFunc, Func<IEventRepository> repositoryFuncFunc)
            : base(sessionFunc, repositoryFuncFunc)
        {
        }

        public override Result Handle(AddBookToLibrary command)
        {

            Library library = Library.CreateFromHistory(EventRepository.GetEventsForAggregate<Library>(command.AggregateId));
            Result result = library.AddBookToLibrary(command.ProcessId, command.Title, command.Author, command.Isbn);
            if (!result.Success) return result;

            EventRepository.Save(library);

            return Success();
        }
    }
}