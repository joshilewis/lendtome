using System;
using System.Collections.Generic;
using System.Security.Claims;
using JWT;

namespace Joshilewis.Infrastructure.Auth
{
    public class Tokeniser
    {
        private readonly string secretKey;

        public Tokeniser(string secretKey)
        {
            this.secretKey = secretKey;
        }

        public string CreateToken(string userName, Guid userId)
        {
            var payload = new Dictionary<string, object>
            {
                {"Expires", DateTime.UtcNow.AddDays(7)},
                {
                    "Claims", new[]
                    {
                        new Claim(ClaimTypes.Name, userName),
                        new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                    }
                }
            };

            return JsonWebToken.Encode(payload, secretKey, JwtHashAlgorithm.HS256);
        }

    }
}
