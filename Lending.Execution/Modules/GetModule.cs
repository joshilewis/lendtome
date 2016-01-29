using Lending.Cqrs;
using Lending.Cqrs.Query;
using Lending.Execution.UnitOfWork;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;

namespace Lending.Execution.Modules
{
    public abstract class GetModule<TMessage, TResult> : NancyModule where TMessage : Message where TResult : Result
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMessageHandler<TMessage, TResult> messageHandler;

        protected GetModule(IUnitOfWork unitOfWork, IMessageHandler<TMessage, TResult> messageHandler, string path, bool secure = false)
        {
            this.unitOfWork = unitOfWork;
            this.messageHandler = messageHandler;

            this.RequiresHttps();
            if (secure)
            {
                this.RequiresAuthentication();
            }

            Get[path] = _ =>
            {
                TMessage request = this.Bind<TMessage>();

                Result response = default(Result);
                unitOfWork.DoInTransaction(() =>
                {
                    response = messageHandler.Handle(request);

                });

                return response;
            };
        }


    }
}
