using System;
using System.Collections.Generic;
using System.Security.Claims;
using JWT;
using Lending.Execution.UnitOfWork;
using Nancy;
using Nancy.SimpleAuthentication;

namespace Lending.Execution.Auth
{
    public class AuthCallbackProvider : IAuthenticationCallbackProvider
    {
        private readonly string secretKey;
        private readonly IUserMapper userMapper;
        private readonly IUnitOfWork unitOfWork;

        public AuthCallbackProvider(IUserMapper userMapper, IUnitOfWork unitOfWork, string secretKey)
        {
            this.userMapper = userMapper;
            this.unitOfWork = unitOfWork;
            this.secretKey = secretKey;
        }

        public dynamic Process(NancyModule nancyModule, AuthenticateCallbackData model)
        {
            AuthenticatedUser user = null;
            unitOfWork.DoInTransaction(() => 
             user = userMapper.MapUser(model.AuthenticatedClient)
            );

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
