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

        protected virtual EResultCode Success()
        {
            return EResultCode.Ok;
        }

        protected virtual EResultCode Created()
        {
            return EResultCode.Created;
        }

        protected virtual EResultCode Fail(string reason)
        {
            throw new InvalidOperationException(reason);
        }

    }
}
