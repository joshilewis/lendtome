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

        private readonly ICheckIfUserHasOpenedLibrary checkIfUserHasOpenedLibrary;

        public LibraryOpener(Func<IEventRepository> eventRepositoryFunc,
            ICheckIfUserHasOpenedLibrary checkIfUserHasOpenedLibrary)
            : base(eventRepositoryFunc)
        {
            this.checkIfUserHasOpenedLibrary = checkIfUserHasOpenedLibrary;
        }

        public override object Handle(OpenLibrary command)
        {
            Library library = Library.Open(command.ProcessId, command.UserId, command.Name, command.UserId);
            EventRepository.Save(library);

            return Created();
        }
    }
}
