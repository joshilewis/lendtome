using System;

namespace Lending.Cqrs.Command
{
    public abstract class AuthenticatedCommandHandler<TRequest, TResponse> : CommandHandler<TRequest, TResponse>, 
        IAuthenticatedCommandHandler<TRequest, TResponse> where TRequest : AuthenticatedCommand
    {
        protected AuthenticatedCommandHandler(Func<IRepository> repositoryFunc, Func<IEventRepository> eventRepositoryFunc)
            : base(repositoryFunc, eventRepositoryFunc)
        {
        }
    }
}
