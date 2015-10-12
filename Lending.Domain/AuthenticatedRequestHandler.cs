using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace Lending.Domain
{
    public abstract class AuthenticatedRequestHandler<TRequest, TResponse> : RequestHandler<TRequest, TResponse>, 
        IAuthenticatedRequestHandler<TRequest, TResponse> where TRequest : AuthenticatedRequest
    {
        protected AuthenticatedRequestHandler(Func<ISession> sessionFunc, Func<IRepository> repositoryFunc) : base(sessionFunc, repositoryFunc)
        {
        }
    }
}
