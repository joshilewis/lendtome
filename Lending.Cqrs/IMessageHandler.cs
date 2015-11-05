using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Cqrs.Command;
using Lending.Cqrs.Query;

namespace Lending.Cqrs
{

    public interface IMessageHandler
    {
        object Handle(object message);
    }

    public interface IMessageHandler<in TMessage, out TResult> : IMessageHandler where TMessage : Message where TResult : Result
    {
        TResult Handle(TMessage message);
    }

    public abstract class MessageHandler : IMessageHandler
    {
        public abstract object Handle(object message);
    }

    public abstract class MessageHandler<TMessage, TResult> : MessageHandler, IMessageHandler<TMessage, TResult> where TMessage : Message where TResult : Result
    {
        public abstract TResult Handle(TMessage message);

        public override object Handle(object message)
        {
            return (TResult) Handle((TMessage) message);
        }
    }
}
