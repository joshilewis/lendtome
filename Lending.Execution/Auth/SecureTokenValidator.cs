using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using JWT;
using Owin.StatelessAuth;

namespace Lending.Execution.Auth
{
    public class SecureTokenValidator : ITokenValidator
    {
        const string SecretKey = "30ea254132194749377862e7d9a644c1";

        public ClaimsPrincipal ValidateUser(string token)
        {
            try
            {
                var decodedtoken = JsonWebToken.DecodeToObject(token, SecretKey) as Dictionary<string, object>;
                if (decodedtoken == null)
                    return null;

                DateTime expiry = DateTime.Parse(decodedtoken["Expires"].ToString());
                if (expiry < DateTime.UtcNow) return null;

                var claims = new List<Claim>();

                if (decodedtoken.ContainsKey("Claims"))
                {
                    foreach (Dictionary<string, object> kvp in ((ArrayList) decodedtoken["Claims"]))
                    {
                        claims.Add(new Claim(kvp["Type"].ToString(), kvp["Value"].ToString()));
                    }
                }

                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Token"));

                return claimsPrincipal;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
