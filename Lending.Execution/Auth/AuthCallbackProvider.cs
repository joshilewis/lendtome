using System;
using System.Collections.Generic;
using System.Security.Claims;
using JWT;
using Nancy;
using Nancy.SimpleAuthentication;

namespace Lending.Execution.Auth
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
}
