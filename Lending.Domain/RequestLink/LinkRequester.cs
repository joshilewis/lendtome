using System;
using Joshilewis.Cqrs;
using Joshilewis.Cqrs.Command;
using Joshilewis.Cqrs.Query;
using Lending.Domain.Model;
using Lending.Domain.OpenLibrary;

namespace Lending.Domain.RequestLink
{
    public class LinkRequester : AuthenticatedCommandHandler<RequestLink>
    {
        public const string CantConnectToSelf = "You can't link to yourself";

        public LinkRequester(Func<IEventRepository> eventRepositoryFunc)
            : base(eventRepositoryFunc)
        {
        }

        public override object Handle(RequestLink command)
        {
            if (command.TargetLibraryId == command.AggregateId) return Success();

            Library library = Library.CreateFromHistory(EventRepository.GetEventsForAggregate<Library>(command.AggregateId));
            library.CheckUserAuthorized(new AdministratorId(command.UserId));
            library.RequestLink(command.ProcessId, new LibraryId(command.TargetLibraryId));

            Library targetLibrary = Library.CreateFromHistory(EventRepository.GetEventsForAggregate<Library>(command.TargetLibraryId));
            targetLibrary.ReceiveLinkRequest(command.ProcessId, new LibraryId(command.AggregateId));

            EventRepository.Save(library);
            EventRepository.Save(targetLibrary);

            return Success();
        }

    }
}