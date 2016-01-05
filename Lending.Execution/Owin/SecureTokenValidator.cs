using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JWT;
using Owin.StatelessAuth;

namespace Lending.Execution.Owin
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

                DateTime expiry = DateTime.Parse(decodedtoken["Expiry"].ToString());

                if (expiry < DateTime.UtcNow)
                    return null;

                var claims = new List<Claim>();

                if (decodedtoken.ContainsKey("Claims"))
                {
                    for (var i = 0; i < ((ArrayList)decodedtoken["Claims"]).Count; i++)
                    {
                        var type = ((Dictionary<string, object>)((ArrayList)decodedtoken["Claims"])[i])["Type"].ToString();
                        var value = ((Dictionary<string, object>)((ArrayList)decodedtoken["Claims"])[i])["Value"].ToString();
                        claims.Add(new Claim(type, value));
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
