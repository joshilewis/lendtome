using System;
using System.Net;
using Lending.Cqrs;
using Lending.Cqrs.Command;
using Lending.Cqrs.Query;
using Lending.Domain;
using Lending.Execution.UnitOfWork;
using ServiceStack.Logging;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Lending.Execution.WebServices
{
    public class Webservice<TMessage, TResult> : Service, IWebservice<TMessage, TResult> where TMessage : Message where TResult : Result
    {
        private readonly static ILog Log = LogManager.GetLogger(typeof(Webservice<TMessage, TResult>).FullName);

        private readonly IUnitOfWork unitOfWork;
        private readonly IMessageHandler<TMessage, TResult> messageHandler;

        public Webservice(IUnitOfWork unitOfWork, 
            IMessageHandler<TMessage, TResult> messageHandler)
        {
            this.unitOfWork = unitOfWork;
            this.messageHandler = messageHandler;
        }

        public virtual object Any(TMessage request)
        {
            Log.InfoFormat("Received a request of type {0}", typeof(TMessage));
            TResult response = default(TResult);
                unitOfWork.DoInTransaction(() =>
                {
                    response = messageHandler.Handle(request);
                });

            return response;
        }

    }
}
