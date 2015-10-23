using System;
using NHibernate;

namespace Lending.Cqrs
{
    public abstract class AuthenticatedCommandHandler<TRequest, TResponse> : CommandHandler<TRequest, TResponse>, 
        IAuthenticatedCommandHandler<TRequest, TResponse> where TRequest : AuthenticatedCommand
    {
        protected AuthenticatedCommandHandler(Func<ISession> getSession, Func<IEventRepository> getRepository)
            : base(getSession, getRepository)
        {
        }
    }
}
