using System;

namespace Lending.Domain
{
    public interface IAuthenticatedRequestHandler<in TRequest, out TResponse>
    {
        TResponse HandleRequest(TRequest request, Guid userId);
    }
}
