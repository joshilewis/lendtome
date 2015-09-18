using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Core;
using Lending.Core.NewUser;
using Lending.Execution.Auth;
using NUnit.Framework;
using ServiceStack.Authentication.NHibernate;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;

namespace Lending.Core.NewUser
{
}

namespace Tests.NewUser
{
    public class NewUserRequestHandlerTests : DatabaseFixtureBase
    {
        [Test]
        public void ExistingUserShouldNotBeCreatedAndNoEventEmitted()
        {
            var existingUser = DefaultTestData.ServiceStackUser1;
            SaveEntities(existingUser);//This is needed because ServiceStack will persist the AuthDto behind the scenes on sign-up

            CommitTransactionAndOpenNew();

            var request = new AuthSessionDouble();
            var expectedResponse = new BaseResponse();

            var expectedUser = DefaultTestData.ServiceStackUser1;

            var sut = new NewUserRequestHandler(() => Session, new UnexpectedEventEmitter());
            BaseResponse actualResponse = sut.HandleRequest(request);

            actualResponse.ShouldEqual(expectedResponse);

            CommitTransactionAndOpenNew();

            int numberOfUsersInDb = Session
                .QueryOver<ServiceStackUser>()
                .RowCount()
                ;

            Assert.That(numberOfUsersInDb, Is.EqualTo(1));
        }

        [Test]
        public void NewUserShouldBeCreated()
        {
            var authDto = DefaultTestData.UserAuthPersistenceDto1;
            SaveEntities(authDto);//This is needed because ServiceStack will persist the AuthDto behind the scenes on sign-up

            CommitTransactionAndOpenNew();

            var request = new AuthSessionDouble();
            var expectedResponse = new BaseResponse();

            var expectedUser = DefaultTestData.ServiceStackUser1;
            ExpectedEventEmitter eventEmitter = new ExpectedEventEmitter();
            eventEmitter.ExpectEvent(new UserAddedEvent(authDto.Id, authDto.UserName, authDto.Email));

            var sut = new NewUserRequestHandler(() => Session, eventEmitter);
            BaseResponse actualResponse = sut.HandleRequest(request);

            actualResponse.ShouldEqual(expectedResponse);
            eventEmitter.VerifyExpectations();

            CommitTransactionAndOpenNew();

            ServiceStackUser userInDb = Session
                .QueryOver<ServiceStackUser>()
                .SingleOrDefault()
                ;

            userInDb.ShouldEqual(expectedUser);

        }

        public class AuthSessionDouble : IAuthSession
        {
            public bool HasRole(string role)
            {
                throw new NotImplementedException();
            }

            public bool HasPermission(string permission)
            {
                throw new NotImplementedException();
            }

            public bool IsAuthorized(string provider)
            {
                throw new NotImplementedException();
            }

            public void OnRegistered(IServiceBase registrationService)
            {
                throw new NotImplementedException();
            }

            public void OnAuthenticated(IServiceBase authService, IAuthSession session, IOAuthTokens tokens, Dictionary<string, string> authInfo)
            {
                throw new NotImplementedException();
            }

            public void OnLogout(IServiceBase authService)
            {
                throw new NotImplementedException();
            }

            public void OnCreated(IHttpRequest httpReq)
            {
                throw new NotImplementedException();
            }

            public string ReferrerUrl { get; set; }
            public string Id { get; set; }

            public string UserAuthId
            {
                get { return "1"; }
                set { } 
            }

            public string UserAuthName { get; set; }
            public string UserName { get; set; }
            public string DisplayName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public List<IOAuthTokens> ProviderOAuthAccess { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime LastModified { get; set; }
            public List<string> Roles { get; set; }
            public List<string> Permissions { get; set; }
            public bool IsAuthenticated { get; set; }
            public string Sequence { get; set; }
        }

        public class UnexpectedEventEmitter : IEventEmitter<UserAddedEvent>
        {
            public void EmitEvent(UserAddedEvent userAddedEvent)
            {
                Assert.Fail("UserAddedEvent should not be emitted for existing user");
            }
        }

        public class ExpectedEventEmitter : IEventEmitter<UserAddedEvent>
        {
            private UserAddedEvent expectedEvent;

            public void ExpectEvent(UserAddedEvent expected)
            {
                expectedEvent = expected;
            }

            private bool called = false;
            public void EmitEvent(UserAddedEvent userAddedEvent)
            {
                userAddedEvent.ShouldEqual(expectedEvent);
                called = true;
            }

            public void VerifyExpectations()
            {
                if (!called) Assert.Fail("No event emitted");
            }
        }
    }
}
