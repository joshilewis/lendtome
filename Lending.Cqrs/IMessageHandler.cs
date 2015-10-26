using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Cqrs.Command;
using Lending.Cqrs.Query;

namespace Lending.Cqrs
{
    public interface IMessageHandler<in TMessage, out TResult> where TMessage : Message where TResult : Result
    {
        TResult Handle(TMessage message);
    }
}
