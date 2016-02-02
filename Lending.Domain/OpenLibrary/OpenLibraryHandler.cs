using System;
using Lending.Cqrs;
using Lending.Cqrs.Command;
using Lending.Cqrs.Query;
using Lending.Domain.Model;

namespace Lending.Domain.OpenLibrary
{
    public class OpenLibraryHandler : CommandHandler<OpenLibrary, Result>, IAuthenticatedCommandHandler<OpenLibrary, Result>
    {
        public const string UserAlreadyOpenedLibrary = "User has already opened a library";

        private readonly ICheckIfUserHasOpenedLibrary checkIfUserHasOpenedLibrary;

        public OpenLibraryHandler(Func<IRepository> repositoryFunc, Func<IEventRepository> eventRepositoryFunc,
            ICheckIfUserHasOpenedLibrary checkIfUserHasOpenedLibrary)
            : base(repositoryFunc, eventRepositoryFunc)
        {
            this.checkIfUserHasOpenedLibrary = checkIfUserHasOpenedLibrary;
        }

        public override Result Handle(OpenLibrary command)
        {
            if (checkIfUserHasOpenedLibrary.UserHasOpenedLibrary(command.AdministratorId))
                Fail(UserAlreadyOpenedLibrary);

            Library library = Library.Open(command.ProcessId, command.AggregateId, command.Name, command.AdministratorId);
            EventRepository.Save(library);

            return Created();
        }
    }
}
