using Nancy.SimpleAuthentication;
using SimpleAuthentication.Core;

namespace Lending.Execution.Auth
{
    public interface IUserMapper
    {
        AuthenticatedUser MapUser(IAuthenticatedClient client);
    }
}