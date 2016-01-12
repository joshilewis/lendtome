using System;
using Lending.Cqrs;
using Lending.Cqrs.Command;
using Lending.Cqrs.Query;
using Lending.Domain.Model;
using Lending.Domain.OpenLibrary;

namespace Lending.Domain.RequestLink
{
    public class RequestLinkHandler : AuthenticatedCommandHandler<RequestLink, Result>
    {
        public const string TargetLibraryDoesNotExist = "The target user does not exist";
        public const string CantConnectToSelf = "You can't connect to yourself";

        public RequestLinkHandler(Func<IRepository> repositoryFunc, Func<IEventRepository> eventRepositoryFunc)
            : base(repositoryFunc, eventRepositoryFunc)
        {
        }

        public override Result Handle(RequestLink command)
        {
            if (command.TargetLibraryId == command.AggregateId) return new Result(CantConnectToSelf);

            OpenedLibrary targetOpenedLibrary = Session.Get<OpenedLibrary>(command.TargetLibraryId);
            if (targetOpenedLibrary == null) return new Result(TargetLibraryDoesNotExist);

            Library library = Library.CreateFromHistory(EventRepository.GetEventsForAggregate<Library>(command.AggregateId));
            Result  result = library.RequestLink(command.ProcessId, command.TargetLibraryId);

            if (!result.Success) return result;

            Library targetLibrary = Library.CreateFromHistory(EventRepository.GetEventsForAggregate<Library>(command.TargetLibraryId));
            result = targetLibrary.InitiateLinkAcceptance(command.ProcessId, command.AggregateId);

            if (!result.Success) return result;

            EventRepository.Save(library);
            EventRepository.Save(targetLibrary);

            return Success();
        }

    }
}