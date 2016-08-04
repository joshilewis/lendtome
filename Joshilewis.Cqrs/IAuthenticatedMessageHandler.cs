using Joshilewis.Cqrs.Query;

namespace Joshilewis.Cqrs
{
    public interface IAuthenticatedMessageHandler<in TMessage> : IMessageHandler<TMessage> where TMessage : Message, IAuthenticated
    {
    }
}
