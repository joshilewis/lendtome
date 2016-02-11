using System;
using Joshilewis.Cqrs;
using Joshilewis.Cqrs.Command;
using Joshilewis.Cqrs.Query;
using Lending.Domain.Model;
using Lending.Domain.OpenLibrary;

namespace Lending.Domain.RequestLink
{
    public class RequestLinkHandler : AuthenticatedCommandHandler<RequestLink, Result>
    {
        public const string CantConnectToSelf = "You can't connect to yourself";

        public RequestLinkHandler(Func<IRepository> repositoryFunc, Func<IEventRepository> eventRepositoryFunc)
            : base(repositoryFunc, eventRepositoryFunc)
        {
        }

        public override Result Handle(RequestLink command)
        {
            if (command.TargetLibraryId == command.AggregateId) return Fail(CantConnectToSelf);

            Library library = Library.CreateFromHistory(EventRepository.GetEventsForAggregate<Library>(command.AggregateId));
            library.CheckUserAuthorized(command.UserId);
            Result  result = library.RequestLink(command.ProcessId, command.TargetLibraryId);

            Library targetLibrary = Library.CreateFromHistory(EventRepository.GetEventsForAggregate<Library>(command.TargetLibraryId));
            result = targetLibrary.InitiateLinkAcceptance(command.ProcessId, command.AggregateId);

            EventRepository.Save(library);
            EventRepository.Save(targetLibrary);

            return Success();
        }

    }
}