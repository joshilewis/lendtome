using System;
using Lending.Cqrs.Query;

namespace Lending.Cqrs.Command
{
    public abstract class AuthenticatedCommandHandler<TRequest, TResult> : CommandHandler<TRequest, TResult>, 
        IAuthenticatedCommandHandler<TRequest, TResult> where TRequest : AuthenticatedCommand where TResult : Result
    {
        protected AuthenticatedCommandHandler(Func<IRepository> repositoryFunc, Func<IEventRepository> eventRepositoryFunc)
            : base(repositoryFunc, eventRepositoryFunc)
        {
        }
    }
}
