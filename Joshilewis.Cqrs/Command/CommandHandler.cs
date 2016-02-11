using System;
using Joshilewis.Cqrs.Query;

namespace Joshilewis.Cqrs.Command
{
    public abstract class CommandHandler<TMessage, TResult> : MessageHandler<TMessage, TResult>, 
        ICommandHandler<TMessage, TResult> where TMessage : Command where TResult : Result
    {
        private readonly Func<IRepository> getRepository;
        private readonly Func<IEventRepository> getEventRepository;

        protected CommandHandler(Func<IRepository> repositoryFunc, Func<IEventRepository> eventRepositoryFunc)
        {
            this.getRepository = repositoryFunc;
            this.getEventRepository = eventRepositoryFunc;
        }

        protected IRepository Session => getRepository();

        protected IEventRepository EventRepository => getEventRepository();

        protected virtual Result Success()
        {
            return new Result(Result.EResultCode.Ok);
        }

        protected virtual Result Created()
        {
            return new Result(Result.EResultCode.Created);
        }

        protected virtual Result Fail(string reason)
        {
            throw new InvalidOperationException(reason);
        }

    }
}
