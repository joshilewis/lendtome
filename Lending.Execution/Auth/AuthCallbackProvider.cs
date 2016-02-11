using System;
using System.Collections.Generic;
using System.Security.Claims;
using Joshilewis.Infrastructure.Auth;
using Joshilewis.Infrastructure.UnitOfWork;
using JWT;
using Nancy;
using Nancy.SimpleAuthentication;

namespace Lending.Execution.Auth
{
    public class AuthCallbackProvider : IAuthenticationCallbackProvider
    {
        private readonly Tokeniser tokeniser;
        private readonly IUserMapper userMapper;
        private readonly IUnitOfWork unitOfWork;

        public AuthCallbackProvider(IUserMapper userMapper, IUnitOfWork unitOfWork, Tokeniser tokeniser)
        {
            this.userMapper = userMapper;
            this.unitOfWork = unitOfWork;
            this.tokeniser = tokeniser;
        }

        public dynamic Process(NancyModule nancyModule, AuthenticateCallbackData model)
        {
            AuthenticatedUser user = null;
            unitOfWork.DoInTransaction(() => 
             user = userMapper.MapUser(model.AuthenticatedClient)
            );

            string token = tokeniser.CreateToken(user.UserName, user.Id);

            return new {Token = token};
        }

        public dynamic OnRedirectToAuthenticationProviderError(NancyModule nancyModule, string errorMessage)
        {
            throw new NotImplementedException();
        }
    }
}
