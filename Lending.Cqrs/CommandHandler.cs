using System;

namespace Lending.Cqrs
{
    public abstract class CommandHandler<TRequest, TResponse> : ICommandHandler<TRequest, TResponse>
    {
        private readonly Func<IRepository> getRepository;
        private readonly Func<IEventRepository> getEventRepository;

        protected CommandHandler(Func<IRepository> repositoryFunc, Func<IEventRepository> eventRepositoryFunc)
        {
            this.getRepository = repositoryFunc;
            this.getEventRepository = eventRepositoryFunc;
        }

        public abstract TResponse HandleCommand(TRequest command);

        protected IRepository Session => getRepository();

        protected IEventRepository EventRepository => getEventRepository();

        protected virtual Result Success()
        {
            return new Result();
        }

        protected virtual Result Fail(string reason)
        {
            return new Result(reason);
        }

    }
}
