using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.SimpleAuthentication;

namespace Lending.Execution.Nancy
{
    public class AuthCallbackProvider : IAuthenticationCallbackProvider
    {
        public dynamic Process(NancyModule nancyModule, AuthenticateCallbackData model)
        {
            return null;
            throw new NotImplementedException();
        }

        public dynamic OnRedirectToAuthenticationProviderError(NancyModule nancyModule, string errorMessage)
        {
            throw new NotImplementedException();
        }
    }
}
