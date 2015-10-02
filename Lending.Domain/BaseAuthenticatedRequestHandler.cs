using System;
using NHibernate;

namespace Lending.Domain
{
    public abstract class BaseAuthenticatedRequestHandler<TRequest, TResponse> : IAuthenticatedRequestHandler<TRequest, TResponse>
    {
        private readonly Func<ISession> getSession;

        protected BaseAuthenticatedRequestHandler(Func<ISession> sessionFunc)
        {
            this.getSession = sessionFunc;
        }

        protected BaseAuthenticatedRequestHandler() { }

        protected ISession Session
        {
            get { return getSession(); }
        }

        public abstract TResponse HandleRequest(TRequest request, int userId);
    }
}
