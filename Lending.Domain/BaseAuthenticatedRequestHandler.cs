using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace Lending.Domain
{
    public abstract class BaseAuthenticatedRequestHandler<TRequest, TResponse> : IAuthenticatedRequestHandler<TRequest, TResponse>
    {
        private readonly Func<ISession> getSession;

        protected BaseAuthenticatedRequestHandler(Func<ISession> sessionFunc, IRepository repository)
        {
            this.getSession = sessionFunc;
            this.Repository = repository;
        }

        protected ISession Session => getSession();

        protected IRepository Repository { get; }

        public abstract TResponse HandleRequest(TRequest request, Guid userId);
    }
}
