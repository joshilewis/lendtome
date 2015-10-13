using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace Lending.Domain
{
    public abstract class AuthenticatedCommandHandler<TRequest, TResponse> : CommandHandler<TRequest, TResponse>, 
        IAuthenticatedCommandHandler<TRequest, TResponse> where TRequest : AuthenticatedCommand
    {
        protected AuthenticatedCommandHandler(Func<ISession> sessionFunc, Func<IRepository> repositoryFunc) : base(sessionFunc, repositoryFunc)
        {
        }
    }
}
