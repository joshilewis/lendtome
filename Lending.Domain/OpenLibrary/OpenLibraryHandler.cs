using System;
using Lending.Cqrs;
using Lending.Cqrs.Command;
using Lending.Cqrs.Query;
using Lending.Domain.Model;

namespace Lending.Domain.OpenLibrary
{
    public class OpenLibraryHandler : CommandHandler<OpenLibrary, Result>, IAuthenticatedCommandHandler<OpenLibrary, Result>
    {

        public OpenLibraryHandler(Func<IRepository> repositoryFunc, Func<IEventRepository> eventRepositoryFunc)
            : base(repositoryFunc, eventRepositoryFunc)
        {
        }

        public override Result Handle(OpenLibrary command)
        {
            Library library = Library.Open(command.ProcessId, command.AggregateId, command.Name, command.AdministratorId);
            EventRepository.Save(library);

            return Success();
        }
    }
}
