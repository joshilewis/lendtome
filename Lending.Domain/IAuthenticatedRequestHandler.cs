namespace Lending.Domain
{
    public interface IAuthenticatedRequestHandler<in TRequest, out TResponse>
    {
        TResponse HandleRequest(TRequest request, int userId);
    }
}
