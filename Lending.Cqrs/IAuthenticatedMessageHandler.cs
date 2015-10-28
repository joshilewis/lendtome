using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Cqrs.Query;

namespace Lending.Cqrs
{
    public interface IAuthenticatedMessageHandler<in TMessage, out TResult> : IMessageHandler<TMessage, TResult> where TMessage : Message, IAuthenticated
        where TResult : Result
    {
    }
}
