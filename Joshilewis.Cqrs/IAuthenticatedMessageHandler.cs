using Joshilewis.Cqrs.Query;

namespace Joshilewis.Cqrs
{
    public interface IAuthenticatedMessageHandler<in TMessage> : IMessageHandler<TMessage, EResultCode> where TMessage : Message, IAuthenticated
    {
    }
}
