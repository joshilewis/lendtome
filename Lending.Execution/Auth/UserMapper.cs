using System;
using System.Collections.Generic;
using Lending.Domain.OpenLibrary;
using NHibernate;
using SimpleAuthentication.Core;

namespace Lending.Execution.Auth
{
    public class UserMapper : IUserMapper
    {
        private readonly Func<ISession> getSession;

        public UserMapper(Func<ISession> sessionFunc)
        {
            this.getSession = sessionFunc;
        }

        public AuthenticatedUser MapUser(IAuthenticatedClient client)
        {
            ISession session = getSession();
            AuthenticatedUser user = session.QueryOver<AuthenticatedUser>()
                .JoinQueryOver<AuthenticationProvider>(x => x.AuthenticationProviders)
                .Where(y => y.Name == client.ProviderName)
                .Where(y => y.UserId == client.UserInformation.Id)
                .SingleOrDefault();

            if (user != null) return user;

            user = new AuthenticatedUser(SequentialGuid.NewGuid(), client.UserInformation.Name, client.UserInformation.Email, client.UserInformation.Picture,
                new List<AuthenticationProvider>()
                {
                    new AuthenticationProvider(SequentialGuid.NewGuid(), client.ProviderName, client.UserInformation.Id),
                });

            session.Save(user);
            return user;

        }
    }
}