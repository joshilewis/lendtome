using Joshilewis.Cqrs.Query;

namespace Joshilewis.Cqrs
{

    public interface IMessageHandler
    {
        object Handle(object message);
    }

    public interface IMessageHandler<in TMessage> : IMessageHandler where TMessage : Message
    {
        object Handle(TMessage message);
    }

    public abstract class MessageHandler : IMessageHandler
    {
        public abstract object Handle(object message);
    }

    public abstract class MessageHandler<TMessage> : MessageHandler, IMessageHandler<TMessage> where TMessage : Message
    {
        public abstract object Handle(TMessage message);

        public override object Handle(object message)
        {
            return Handle((TMessage) message);
        }
    }

}
