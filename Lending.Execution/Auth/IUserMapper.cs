using Nancy.SimpleAuthentication;

namespace Lending.Execution.Auth
{
    public interface IUserMapper
    {
        AuthenticatedUser MapUser(AuthenticateCallbackData authenticateCallbackData);
    }
}