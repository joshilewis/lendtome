using System;
using System.Net;
using Lending.Cqrs;
using Lending.Cqrs.Command;
using Lending.Cqrs.Query;
using Lending.Domain;
using Lending.Domain.RegisterUser;
using Lending.Execution.Auth;
using Lending.Execution.UnitOfWork;
using NHibernate;
using ServiceStack.Logging;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Lending.Execution.WebServices
{
    public class AuthenticatedWebservice<TMessage, TResult> : Service, IAuthenticatedWebservice<TMessage, TResult>
        where TMessage : Message, IAuthenticated where TResult : Result
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (Webservice<TMessage, TResult>).FullName);

        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthenticatedMessageHandler<TMessage, TResult> messageHandler;

        public AuthenticatedWebservice(IUnitOfWork unitOfWork,
            IAuthenticatedMessageHandler<TMessage, TResult> messageHandler)
        {
            this.unitOfWork = unitOfWork;
            this.messageHandler = messageHandler;
        }

        [Authenticate]
        public virtual object Any(TMessage message)
        {
            Log.InfoFormat("Received a request of type {0}", typeof (TMessage));

            int authUserId = int.Parse(this.GetSession().Id);

            TResult response = default(TResult);
            unitOfWork.DoInTransaction(() =>
            {
                ISession session = unitOfWork.CurrentSession;
                RegisteredUser user = session.QueryOver<RegisteredUser>()
                    .Where(x => x.AuthUserId == authUserId)
                    .SingleOrDefault();

                response = messageHandler.Handle(message);
            });

            return response;
        }

    }
}
