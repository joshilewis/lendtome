using System;
using Lending.Cqrs;
using Lending.Cqrs.Command;
using Lending.Cqrs.Query;
using Lending.Domain.Model;

namespace Lending.Domain.AcceptLink
{
    public class AcceptLinkHandler : AuthenticatedCommandHandler<AcceptLink, Result, Library>
    {
        public AcceptLinkHandler(Func<IRepository> repositoryFunc, Func<IEventRepository> eventRepositoryFunc) 
            : base(repositoryFunc, eventRepositoryFunc)
        {
        }

        public override Result Handle(AcceptLink command)
        {
            Library acceptingLibrary = Library.CreateFromHistory(EventRepository.GetEventsForAggregate<Library>(command.AggregateId));

            Result result = acceptingLibrary.AcceptLink(command.ProcessId, command.RequestingLibraryId);

            if (!result.Success) return result;

            Library requestingLibrary = Library.CreateFromHistory(EventRepository.GetEventsForAggregate<Library>(command.RequestingLibraryId));

            result = requestingLibrary.CompleteLink(command.ProcessId, command.AggregateId);
            if (!result.Success) return result;

            EventRepository.Save(acceptingLibrary);
            EventRepository.Save(requestingLibrary);

            return Success();
        }
    }
}