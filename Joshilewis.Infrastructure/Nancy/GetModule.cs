using Joshilewis.Cqrs;
using Joshilewis.Cqrs.Query;
using Joshilewis.Infrastructure.UnitOfWork;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;

namespace Joshilewis.Infrastructure.Nancy
{
    public abstract class GetModule<TMessage, TResult> : NancyModule where TMessage : Message
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMessageHandler<TMessage> messageHandler;

        protected GetModule(IUnitOfWork unitOfWork, IMessageHandler<TMessage> messageHandler, string path)
        {
            this.unitOfWork = unitOfWork;
            this.messageHandler = messageHandler;

            this.RequiresHttps();

            Get[path] = _ =>
            {
                TMessage request = this.Bind<TMessage>();

                object response = null;
                unitOfWork.DoInTransaction(() =>
                {
                    response = messageHandler.Handle(request);

                });

                return response;
            };
        }


    }
}
