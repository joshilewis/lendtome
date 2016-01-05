using Lending.Cqrs;
using Lending.Cqrs.Query;
using Lending.Execution.Auth;
using Lending.Execution.UnitOfWork;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;

namespace Lending.Execution.Modules
{
    public abstract class PostModule<TMessage, TResult> : NancyModule where TMessage : Message where TResult : Result
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMessageHandler<TMessage, TResult> messageHandler;
        protected abstract string Path { get; }

        protected PostModule(IUnitOfWork unitOfWork, IMessageHandler<TMessage, TResult> messageHandler)
            : base("nancy")
        {
            this.unitOfWork = unitOfWork;
            this.messageHandler = messageHandler;

            this.RequiresAuthentication();
            //this.RequiresHttps();

            Post[Path] = _ =>
            {
                CustomUserIdentity user = this.Context.CurrentUser as CustomUserIdentity;

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
