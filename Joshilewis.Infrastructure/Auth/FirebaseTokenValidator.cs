using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Owin.StatelessAuth;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;


namespace Joshilewis.Infrastructure.Auth
{
    public class FirebaseTokenValidator : ITokenValidator
    {
        public ClaimsPrincipal ValidateUser(string token)
        {
            HttpClient client = new HttpClient {BaseAddress = new Uri("https://www.googleapis.com/robot/v1/metadata/")};

            // 1. Get Google signing keys
            HttpResponseMessage response = client.GetAsync("x509/securetoken@system.gserviceaccount.com").Result;
            if (!response.IsSuccessStatusCode) { return null; }
            var x509Data = response.Content.ReadAsAsync<Dictionary<string, string>>().Result;
            SecurityKey[] keys = x509Data.Values.Select(CreateSecurityKeyFromPublicKey).ToArray();
            // 2. Configure validation parameters
            const string firebaseProjectId = "lendtome-93c5c";
            var parameters = new TokenValidationParameters
            {
                ValidIssuer = "https://securetoken.google.com/" + firebaseProjectId,
                ValidAudience = firebaseProjectId,
                IssuerSigningKeys = keys,
            };
            // 3. Use JwtSecurityTokenHandler to validate signature, issuer, audience and lifetime
            var handler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            ClaimsPrincipal principal = handler.ValidateToken(token, parameters, out securityToken);
            var jwt = (JwtSecurityToken)securityToken;
            // 4.Validate signature algorithm and other applicable valdiations
            if (jwt.Header.Alg != SecurityAlgorithms.RsaSha256)
            {
                throw new SecurityTokenInvalidSignatureException(
                    "The token is not signed with the expected algorithm.");
            }
            return principal;
        }

        private SecurityKey CreateSecurityKeyFromPublicKey(string data)
        {
            return new X509SecurityKey(new X509Certificate2(Encoding.UTF8.GetBytes(data)));
        }

    }
}
