using Lending.Cqrs;
using Lending.Cqrs.Query;

namespace Lending.Execution.WebServices
{
    public interface IWebservice<TMessage, TResult> where TMessage : Message where TResult : Result
    { }
}