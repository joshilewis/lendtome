using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JWT;
using Nancy;
using Nancy.SimpleAuthentication;

namespace Lending.Execution.Nancy
{
    public class AuthCallbackProvider : IAuthenticationCallbackProvider
    {
        private readonly string secretKey;
        private readonly IUserMapper userMapper;

        public AuthCallbackProvider(IUserMapper userMapper)
        {
            this.userMapper = userMapper;
            this.secretKey = "30ea254132194749377862e7d9a644c1";
        }

        public dynamic Process(NancyModule nancyModule, AuthenticateCallbackData model)
        {
            var user = userMapper.MapUser(model);

            var payload = new Dictionary<string, object>
            {
                {"Expires", DateTime.UtcNow.AddDays(7)},
                {"Claims",  new []
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }}
            };

            var token = JsonWebToken.Encode(payload, secretKey, JwtHashAlgorithm.HS256);

            return new {Token = token};
        }

        public dynamic OnRedirectToAuthenticationProviderError(NancyModule nancyModule, string errorMessage)
        {
            throw new NotImplementedException();
        }
    }

    public interface IUserMapper
    {
        AuthenticatedUser MapUser(AuthenticateCallbackData authenticateCallbackData);
    }

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
