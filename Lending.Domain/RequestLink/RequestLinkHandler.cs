using System;
using Joshilewis.Cqrs;
using Joshilewis.Cqrs.Command;
using Joshilewis.Cqrs.Query;
using Lending.Domain.Model;
using Lending.Domain.OpenLibrary;

namespace Lending.Domain.RequestLink
{
    public class RequestLinkHandler : AuthenticatedCommandHandler<RequestLink>
    {
        public const string CantConnectToSelf = "You can't connect to yourself";

        public RequestLinkHandler(Func<IEventRepository> eventRepositoryFunc)
            : base(eventRepositoryFunc)
        {
        }

        public override EResultCode Handle(RequestLink command)
        {
            if (command.TargetLibraryId == command.AggregateId) return Fail(CantConnectToSelf);

            Library library = Library.CreateFromHistory(EventRepository.GetEventsForAggregate<Library>(command.AggregateId));
            library.CheckUserAuthorized(command.UserId);
            library.RequestLink(command.ProcessId, command.TargetLibraryId);

            Library targetLibrary = Library.CreateFromHistory(EventRepository.GetEventsForAggregate<Library>(command.TargetLibraryId));
            targetLibrary.InitiateLinkAcceptance(command.ProcessId, command.AggregateId);

            EventRepository.Save(library);
            EventRepository.Save(targetLibrary);

            return Success();
        }

    }
}