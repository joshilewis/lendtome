using System;
using Joshilewis.Cqrs;
using Joshilewis.Cqrs.Command;
using Joshilewis.Cqrs.Query;
using Joshilewis.Infrastructure.Auth;
using Joshilewis.Infrastructure.UnitOfWork;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;

namespace Joshilewis.Infrastructure.Nancy
{
    public abstract class PostModule<TMessage> : NancyModule where TMessage : AuthenticatedCommand
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICommandHandler<TMessage> commandHandler;

        protected PostModule(IUnitOfWork unitOfWork, ICommandHandler<TMessage> commandHandler, string path)
        {
            this.unitOfWork = unitOfWork;
            this.commandHandler = commandHandler;

            this.RequiresAuthentication();
            this.RequiresHttps();

            Post[path] = _ =>
            {
                CustomUserIdentity user = this.Context.CurrentUser as CustomUserIdentity;

                TMessage message = this.Bind<TMessage>();
                message.UserId = user.Id;
                message.ProcessId = Guid.NewGuid();

                EResultCode resultCode = default(EResultCode);
                unitOfWork.DoInTransaction(() =>
                {
                    resultCode = commandHandler.Handle(message);
                });

                return new Response()
                {
                    StatusCode = (HttpStatusCode) resultCode,
                };

            };
        }
    }
}
