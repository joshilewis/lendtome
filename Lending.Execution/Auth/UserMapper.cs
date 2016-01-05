using System;
using Nancy.SimpleAuthentication;

namespace Lending.Execution.Auth
{
    public class UserMapper : IUserMapper
    {
        public AuthenticatedUser MapUser(AuthenticateCallbackData authenticateCallbackData)
        {
            return new AuthenticatedUser(Guid.NewGuid(), authenticateCallbackData.AuthenticatedClient.UserInformation.Name, new[]
            {
                new AuthenticationProvider(authenticateCallbackData.AuthenticatedClient.ProviderName,
                    authenticateCallbackData.AuthenticatedClient.UserInformation.Id),
            });
        }
    }
}