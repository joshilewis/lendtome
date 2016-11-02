using System;
using Joshilewis.Cqrs;
using Joshilewis.Cqrs.Command;
using Joshilewis.Cqrs.Query;
using Lending.Domain.Model;

namespace Lending.Domain.AcceptLink
{
    public class LinkAccepter : AuthenticatedCommandHandler<AcceptLink>
    {
        public LinkAccepter(Func<IEventRepository> eventRepositoryFunc) 
            : base(eventRepositoryFunc)
        {
        }

        public override object Handle(AcceptLink command)
        {
            Library acceptingLibrary = Library.CreateFromHistory(EventRepository.GetEventsForAggregate<Library>(command.AggregateId));
            acceptingLibrary.CheckUserAuthorized(new AdministratorId(command.UserId));

            acceptingLibrary.AcceptLink(command.ProcessId, new LibraryId(command.RequestingLibraryId));

            Library requestingLibrary = Library.CreateFromHistory(EventRepository.GetEventsForAggregate<Library>(command.RequestingLibraryId));

            requestingLibrary.CompleteLink(command.ProcessId, new LibraryId(command.AggregateId));

            EventRepository.Save(acceptingLibrary);
            EventRepository.Save(requestingLibrary);

            return Success();
        }
    }
}