using System;
using Joshilewis.Cqrs;
using Joshilewis.Cqrs.Command;
using Joshilewis.Cqrs.Query;
using Lending.Domain.Model;

namespace Lending.Domain.AcceptLink
{
    public class AcceptLinkHandler : AuthenticatedCommandHandler<AcceptLink>
    {
        public AcceptLinkHandler(Func<IEventRepository> eventRepositoryFunc) 
            : base(eventRepositoryFunc)
        {
        }

        public override EResultCode Handle(AcceptLink command)
        {
            Library acceptingLibrary = Library.CreateFromHistory(EventRepository.GetEventsForAggregate<Library>(command.AggregateId));
            acceptingLibrary.CheckUserAuthorized(command.UserId);

            acceptingLibrary.AcceptLink(command.ProcessId, command.RequestingLibraryId);

            Library requestingLibrary = Library.CreateFromHistory(EventRepository.GetEventsForAggregate<Library>(command.RequestingLibraryId));

            requestingLibrary.CompleteLink(command.ProcessId, command.AggregateId);

            EventRepository.Save(acceptingLibrary);
            EventRepository.Save(requestingLibrary);

            return Success();
        }
    }
}