using System;
using System.IO;
using System.Runtime.Remoting.Messaging;
using Joshilewis.Cqrs;
using Joshilewis.Cqrs.Command;
using Joshilewis.Cqrs.Query;
using Joshilewis.Infrastructure.Auth;
using Joshilewis.Infrastructure.EventRouting;
using Joshilewis.Infrastructure.UnitOfWork;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;

namespace Joshilewis.Infrastructure.Nancy
{
    public abstract class PostModule<TMessage> : NancyModule where TMessage : AuthenticatedCommand
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly NHibernateUnitOfWork relationalUnitOfWork;
        private readonly ICommandHandler<TMessage> commandHandler;
        private readonly EventDispatcher eventDispatcher;

        protected PostModule(IUnitOfWork unitOfWork, ICommandHandler<TMessage> commandHandler, string path,
            NHibernateUnitOfWork relationalUnitOfWork, EventDispatcher eventDispatcher)
        {
            this.unitOfWork = unitOfWork;
            this.commandHandler = commandHandler;
            this.relationalUnitOfWork = relationalUnitOfWork;
            this.eventDispatcher = eventDispatcher;

            this.RequiresAuthentication();
            this.RequiresHttps();

            Post[path] = _ =>
            {
                CustomUserIdentity user = this.Context.CurrentUser as CustomUserIdentity;

                TMessage message = this.Bind<TMessage>();
                message.UserId = user.Id;
                message.ProcessId = Guid.NewGuid();

                Result result = null;
                //EResultCode resultCode = default(EResultCode);
                unitOfWork.DoInTransaction(() =>
                {
                    result = (Result) commandHandler.Handle(message);
                });

                relationalUnitOfWork.DoInTransaction(eventDispatcher.DispatchEvents);

                return Response.AsJson(result.Payload, (HttpStatusCode)result.Code);
            };
        }

    }
}
