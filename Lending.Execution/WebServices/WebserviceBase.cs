using System;
using System.Net;
using Lending.Domain;
using Lending.Execution.UnitOfWork;
using ServiceStack.Logging;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace Lending.Execution.WebServices
{
    public interface IWebserviceBase<TRequest, TResponse>
    { }

    //[Authenticate]
    public class WebserviceBase<TRequest, TResponse> : Service, IWebserviceBase<TRequest, TResponse>
    {
        private readonly static ILog Log = LogManager.GetLogger(typeof(WebserviceBase<TRequest, TResponse>).FullName);

        private readonly IUnitOfWork unitOfWork;
        private readonly ICommandHandler<TRequest, TResponse> commandHandler;

        public WebserviceBase(IUnitOfWork unitOfWork, 
            ICommandHandler<TRequest, TResponse> commandHandler)
        {
            this.unitOfWork = unitOfWork;
            this.commandHandler = commandHandler;
        }

        public virtual object Any(TRequest request)
        {
            Log.InfoFormat("Received a request of type {0}", typeof(TRequest));
            TResponse response = default(TResponse);
                unitOfWork.DoInTransaction(() =>
                {
                    response = commandHandler.HandleCommand(request);
                });

            return response;
        }

    }
}
