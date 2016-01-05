using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JWT;
using Nancy;
using Nancy.SimpleAuthentication;
using Nancy.Authentication.Forms;
using Nancy.Authentication.Token;

namespace Lending.Execution.Nancy
{
    public class AuthCallbackProvider : IAuthenticationCallbackProvider
    {
        private readonly string secretKey;

        public AuthCallbackProvider()
        {
            this.secretKey = "30ea254132194749377862e7d9a644c1";
        }

        public dynamic Process(NancyModule nancyModule, AuthenticateCallbackData model)
        {
            //return nancyModule.LoginAndRedirect(Guid.NewGuid());

            var user = new User(Guid.NewGuid(), model.AuthenticatedClient.UserInformation.Name, new[]
            {
                new AuthenticationProvider(model.AuthenticatedClient.ProviderName,
                    model.AuthenticatedClient.UserInformation.Id),
            });

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
