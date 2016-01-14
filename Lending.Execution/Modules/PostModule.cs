using System;
using Lending.Cqrs;
using Lending.Cqrs.Command;
using Lending.Cqrs.Exceptions;
using Lending.Cqrs.Query;
using Lending.Execution.Auth;
using Lending.Execution.UnitOfWork;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;

namespace Lending.Execution.Modules
{
    public abstract class PostModule<TMessage, TResult> : NancyModule where TMessage : AuthenticatedCommand where TResult : Result
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMessageHandler<TMessage, TResult> messageHandler;
        protected abstract string Path { get; }

        protected PostModule(IUnitOfWork unitOfWork, IMessageHandler<TMessage, TResult> messageHandler)
            : base("api")
        {
            this.unitOfWork = unitOfWork;
            this.messageHandler = messageHandler;

            this.RequiresAuthentication();
            //this.RequiresHttps();

            Post[Path] = _ =>
            {
                CustomUserIdentity user = this.Context.CurrentUser as CustomUserIdentity;

                TMessage message = this.Bind<TMessage>();
                message.UserId = user.Id;
                message.ProcessId = Guid.NewGuid();

                try
                {
                    Result result = default(Result);
                    unitOfWork.DoInTransaction(() =>
                    {
                        result = messageHandler.Handle(message);
                    });

                    return new Response()
                    {
                        StatusCode = (HttpStatusCode)result.Code,
                    };
                }
                catch (AggregateNotFoundException aggregateNotFoundException)
                {
                    return new NotFoundResponse()
                    {
                        ReasonPhrase = aggregateNotFoundException.Message,
                    };
                }
                catch (InvalidOperationException invalidOperationException)
                {
                    return new Response()
                    {
                        ReasonPhrase = invalidOperationException.Message,
                        StatusCode = HttpStatusCode.BadRequest,
                    };
                }

            };
        }
    }
}
