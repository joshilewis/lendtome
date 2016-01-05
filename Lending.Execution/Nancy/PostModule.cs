using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Cqrs;
using Lending.Cqrs.Query;
using Lending.Execution.UnitOfWork;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;

namespace Lending.Execution.Nancy
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
                AuthenticatedUser user = this.Context.CurrentUser as AuthenticatedUser;

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
