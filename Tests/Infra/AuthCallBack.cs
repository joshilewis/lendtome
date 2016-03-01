using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Domain.OpenLibrary;
using Lending.Execution.Auth;
using NUnit.Framework;
using SimpleAuthentication.Core;
using static Joshilewis.Testing.Helpers.PersistenceExtensions;

namespace Tests.Infra
{
    [TestFixture]
    public class AuthCallBack : Fixture
    {
        [Test]
        public void ExistingUserAuthenticatingShouldUseExistingUser()
        {
            var authenticatedUser = new AuthenticatedUser(Guid.NewGuid(), "user1", "email1", "picture", new List<AuthenticationProvider>()
            {
                new AuthenticationProvider(Guid.NewGuid(), "Facebook", "12345"),
            });

            var authenticatedClient = new AuthenticatedClient("Facebook")
            {
                UserInformation = new UserInformation()
                {
                    Id = "12345",
                    Name = "user1",
                }
            };
            SaveEntities(authenticatedUser);

            var sut = new UserMapper(() => Session);
            AuthenticatedUser actual = sut.MapUser(authenticatedClient);

            actual.ShouldEqual(authenticatedUser);

            int count = Session.QueryOver<AuthenticatedUser>()
                .RowCount();

            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void NewUserAuthenticatingShouldCreateNewUser()
        {
            var authenticatedUser = new AuthenticatedUser(Guid.NewGuid(), "user1", "Email1", "picture", new List<AuthenticationProvider>()
            {
                new AuthenticationProvider(Guid.NewGuid(), "Facebook", "12345"),
            });

            var authenticatedClient = new AuthenticatedClient("Facebook")
            {
                UserInformation = new UserInformation()
                {
                    Id = "12345",
                    Name = "user1",
                }
            };

            var sut = new UserMapper(() => Session);
            AuthenticatedUser actual = sut.MapUser(authenticatedClient);

            actual.ShouldEqual(authenticatedUser);

            int count = Session.QueryOver<AuthenticatedUser>()
                .RowCount();

            Assert.That(count, Is.EqualTo(1));

            AuthenticatedUser userInDb = Session.QueryOver<AuthenticatedUser>()
                .Where(x => x.UserName == authenticatedUser.UserName)
                .SingleOrDefault();

            userInDb.ShouldEqual(authenticatedUser);

        }

    }
}
