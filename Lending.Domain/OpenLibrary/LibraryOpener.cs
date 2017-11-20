using System;
using Joshilewis.Cqrs;
using Joshilewis.Cqrs.Command;
using Joshilewis.Cqrs.Query;
using Lending.Domain.Model;

namespace Lending.Domain.OpenLibrary
{
    public class LibraryOpener : CommandHandler<OpenLibrary>, IAuthenticatedCommandHandler<OpenLibrary>
    {
        public const string UserAlreadyOpenedLibrary = "User has already opened a library";

        public LibraryOpener(Func<IEventRepository> eventRepositoryFunc)
            : base(eventRepositoryFunc)
        {
        }

        public override object Handle(OpenLibrary command)
        {
            var libraryId = Guid.NewGuid();
            Library library = Library.Open(command.ProcessId, new LibraryId(libraryId), command.Name,
                new AdministratorId(command.UserId), command.Picture);
            EventRepository.Save(library);
            return Created(libraryId.ToString());
        }
    }
}
