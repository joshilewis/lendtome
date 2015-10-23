using System;
using Lending.Cqrs;
using Lending.Domain.Model;

namespace Lending.Domain.RemoveBookFromLibrary
{
    public class RemoveBookFromLibraryHandler : AuthenticatedCommandHandler<RemoveBookFromLibrary, Result>
    {
        public RemoveBookFromLibraryHandler(Func<IRepository> repositoryFunc, Func<IEventRepository> eventRepositoryFunc)
            : base(repositoryFunc, eventRepositoryFunc)
        {
        }

        public override Result HandleCommand(RemoveBookFromLibrary command)
        {
            User user = User.CreateFromHistory(EventRepository.GetEventsForAggregate<User>(command.AggregateId));
            Result result = user.RemoveBookFromLibrary(command.ProcessId, command.Title, command.Author, command.Isbn);

            if (!result.Success) return result;

            EventRepository.Save(user);
            return Success();
        }
    }
}