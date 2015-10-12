using System;

namespace Lending.Domain
{
    public interface IAuthenticatedRequestHandler<in TRequest, out TResponse> where TRequest : AuthenticatedRequest
    {
        TResponse HandleRequest(TRequest request);
    }
}
