using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventStore.ClientAPI;
using Lending.Cqrs;
using Lending.Cqrs.Command;
using Lending.Cqrs.Query;
using Lending.Domain.RegisterUser;
using Lending.Execution.Auth;
using NUnit.Framework;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.Text;

namespace Tests.Domain
{
    public class RegisterUserTests : FixtureWithEventStoreAndNHibernate
    {
        [Test]
        public void RegisterUserForExistingUserShouldFail()
        {
            ServiceStackUser existingUser = DefaultTestData.ServiceStackUser1;
            SaveEntities(existingUser);

            CommitTransactionAndOpenNew();

            var request = new AuthSessionDouble();
            var expectedResponse = new Result();

            var sut = new RegisterUserHandler(() => Session, () => new UnexpectedEventRepository(), () => Guid.Empty);
            Result actualResult = sut.Handle(request);

            actualResult.ShouldEqual(expectedResponse);

            CommitTransactionAndOpenNew();

            int numberOfUsersInDb = Session
                .QueryOver<ServiceStackUser>()
                .RowCount()
                ;

            Assert.That(numberOfUsersInDb, Is.EqualTo(1));

        }

        [Test]
        public void RegisterUserForNewUserShouldSucceed()
        {
            var authDto = DefaultTestData.UserAuthPersistenceDto1;
            SaveEntities(authDto);//This is needed because ServiceStack will persist the AuthDto behind the scenes on sign-up

            CommitTransactionAndOpenNew();

            var userId = Guid.NewGuid();
            var stream = $"user-{userId}";
            var request = new AuthSessionDouble();
            var expectedResponse = new Result();
            var expectedUser = new ServiceStackUser(authDto.Id, userId, authDto.DisplayName);
            var expectedEvent = new UserRegistered(Guid.Empty, userId, authDto.DisplayName, authDto.PrimaryEmail);

            var sut = new RegisterUserHandler(() => Session, () => EventRepository, () => userId);
            Result actualResult = sut.Handle(request);

            actualResult.ShouldEqual(expectedResponse);

            CommitTransactionAndOpenNew();
            WriteRepository();

            ServiceStackUser userInDb = Session
                .QueryOver<ServiceStackUser>()
                .SingleOrDefault()
                ;

            userInDb.ShouldEqual(expectedUser);

            StreamEventsSlice slice = Connection.ReadStreamEventsBackwardAsync(stream, 0, 10, false).Result;
            Assert.That(slice.Events.Count(), Is.EqualTo(1));
            var value = Encoding.UTF8.GetString(slice.Events[0].Event.Data);
            UserRegistered actual = value.FromJson<UserRegistered>();
            actual.ShouldEqual(expectedEvent);
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

        public class UnexpectedEventRepository : IEventRepository
        {
            public IEnumerable<Event> GetEventsForAggregate<TAggregate>(Guid id) where TAggregate : Aggregate
            {
                throw new NotImplementedException();
            }

            public IEnumerable<Event> GetEventsForAggregate<TAggregate>(Guid id, int version) where TAggregate : Aggregate
            {
                throw new NotImplementedException();
            }

            public void Save(Aggregate aggregate)
            {
                Assert.Fail("UserAdded should not be emitted for existing user");
            }

        }
    }
}
