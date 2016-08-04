using System;
using Joshilewis.Cqrs.Query;

namespace Joshilewis.Cqrs.Command
{
    public abstract class AuthenticatedCommandHandler<TRequest> : CommandHandler<TRequest>, 
        IAuthenticatedCommandHandler<TRequest> where TRequest : AuthenticatedCommand
    {
        protected AuthenticatedCommandHandler(Func<IEventRepository> eventRepositoryFunc)
            : base(eventRepositoryFunc)
        {
        }
    }
}
