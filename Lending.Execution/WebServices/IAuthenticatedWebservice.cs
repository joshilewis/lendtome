using Lending.Cqrs;
using Lending.Cqrs.Query;

namespace Lending.Execution.WebServices
{
    public interface IAuthenticatedWebservice<TMessage, TResult> : IWebservice<TMessage, TResult>
        where TMessage : Message, IAuthenticated where TResult : Result
    {
    }
}
