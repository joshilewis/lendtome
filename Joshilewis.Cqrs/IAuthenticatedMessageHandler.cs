using Joshilewis.Cqrs.Query;

namespace Joshilewis.Cqrs
{
    public interface IAuthenticatedMessageHandler<in TMessage, out TResult> : IMessageHandler<TMessage, TResult> where TMessage : Message, IAuthenticated
    {
    }
}
