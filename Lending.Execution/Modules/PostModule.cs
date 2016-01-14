using System;
using System.Collections.Generic;
using Lending.Cqrs;
using Lending.Cqrs.Command;
using Lending.Cqrs.Query;
using Lending.Execution.Auth;
using Lending.Execution.EventStore;
using Lending.Execution.UnitOfWork;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;

namespace Lending.Execution.Modules
{
    public abstract class PostModule<TMessage, TResult, TAggregate> : NancyModule where TMessage : AuthenticatedCommand
        where TResult : Result
        where TAggregate : Aggregate
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICommandHandler<TMessage, TResult, TAggregate> messageHandler;
        private readonly Func<IEventRepository> getEventRepository;
        protected abstract string Path { get; }

        protected PostModule(IUnitOfWork unitOfWork, ICommandHandler<TMessage, TResult, TAggregate> messageHandler,
            Func<IEventRepository> getEventRepository)
            : base("api")
        {
            this.unitOfWork = unitOfWork;
            this.messageHandler = messageHandler;
            this.getEventRepository = getEventRepository;

            this.RequiresAuthentication();
            //this.RequiresHttps();

            Post[Path] = _ =>
            {
                TMessage message = this.Bind<TMessage>();

                CustomUserIdentity user = this.Context.CurrentUser as CustomUserIdentity;

                message.UserId = user.Id;
                message.ProcessId = Guid.NewGuid();

                dynamic result = default(Result);
                unitOfWork.DoInTransaction(() =>
                {
                    try
                    {
                        IEventRepository eventRepository = getEventRepository();
                        IEnumerable<Event> events = eventRepository.GetEventsForAggregate<TAggregate>(message.AggregateId);
                        result = messageHandler.Handle(message);
                    }
                    catch (AggregateNotFoundException)
                    {
                        result = new NotFoundResponse();
                    }

                });

                return result;

            };
        }
    }
}
