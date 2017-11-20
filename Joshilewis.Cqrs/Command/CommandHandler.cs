using System;
using Joshilewis.Cqrs.Query;

namespace Joshilewis.Cqrs.Command
{
    public abstract class CommandHandler<TMessage> : MessageHandler<TMessage>, 
        ICommandHandler<TMessage> where TMessage : Command
    {
        private readonly Func<IEventRepository> getEventRepository;

        protected CommandHandler(Func<IEventRepository> eventRepositoryFunc)
        {
            this.getEventRepository = eventRepositoryFunc;
        }

        protected IEventRepository EventRepository => getEventRepository();

        protected virtual Result Success()
        {
            return new Result(EResultCode.Ok);
        }

        protected virtual Result Success(object payload)
        {
            return new Result(EResultCode.Ok, payload);
        }

        protected virtual Result Created()
        {
            return new Result(EResultCode.Created);
        }

        protected virtual Result Created(object payload)
        {
            return new Result(EResultCode.Created, payload);
        }

        protected virtual EResultCode Fail(string reason)
        {
            throw new InvalidOperationException(reason);
        }

    }
}
