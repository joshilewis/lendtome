using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.SimpleAuthentication;
using Nancy.Authentication.Forms;
using Nancy.Authentication.Token;
using Nancy.Security;

namespace Lending.Execution.Nancy
{
    public class AuthCallbackProvider : IAuthenticationCallbackProvider
    {
        private readonly ITokenizer tokenizer;

        public AuthCallbackProvider(ITokenizer tokenizer)
        {
            this.tokenizer = tokenizer;
        }

        public dynamic Process(NancyModule nancyModule, AuthenticateCallbackData model)
        {
            //return nancyModule.LoginAndRedirect(Guid.NewGuid());

            var user = new User(Guid.NewGuid(), model.AuthenticatedClient.UserInformation.Name, new []
            {
                new AuthenticationProvider(model.AuthenticatedClient.ProviderName, model.AuthenticatedClient.UserInformation.Id), 
            });

            return null;
        }

        public dynamic OnRedirectToAuthenticationProviderError(NancyModule nancyModule, string errorMessage)
        {
            throw new NotImplementedException();
        }
    }

    public class User : IUserIdentity
    {
        public virtual Guid Id { get; protected set; }
        public virtual string UserName { get; protected set; }
        public virtual IEnumerable<string> Claims { get; }
        public virtual IList<AuthenticationProvider> AuthenticationProviders { get; protected set; }

        public User(Guid id, string userName, IList<AuthenticationProvider> authenticationProviders)
        {
            Id = id;
            UserName = userName;
            AuthenticationProviders = authenticationProviders;
            Claims = new List<string>();
        }

        protected User()
        {
            Claims = new List<string>();
        }

    }

    public class AuthenticationProvider
    {
        public virtual string Name { get; protected set; }
        public virtual string Id { get; protected set; }

        public AuthenticationProvider(string name, string id)
        {
            Name = name;
            Id = id;
        }

        protected AuthenticationProvider() { }
    }
}
