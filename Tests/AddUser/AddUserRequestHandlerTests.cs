using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.AddUser;
using Core.Model;
using NHibernate;
using NUnit.Framework;
using Rhino.Mocks;

namespace Tests.AddUser
{
    [TestFixture]
    public class AddUserRequestHandlerTests
    {
        [Test]
        public void Test_UserDoesntExist()
        {
            var session = MockRepository.GenerateMock<ISession>();
            var sessionFunc = MockRepository.GenerateMock<Func<ISession>>();
            var query = MockRepository.GenerateMock<UserExistsQuery>();
            var guidGenerator = MockRepository.GenerateMock<Func<Guid>>();

            var guid = Guid.NewGuid();

            var request = new AddUserRequest()
            {
                EmailAddress = "email@example.org",
                UserName = "username",
            };

            var expectedUser = new User(guid, request.UserName, request.EmailAddress);

            guidGenerator.Expect(g => g())
                .Return(guid)
                ;

            query.Expect(q => q.CheckIfUserExists(request.MatchArg()))
                .Return(false)
                ;

            sessionFunc.Expect(f => f())
                .Return(session)
                ;

            session.Expect(s => s.Save(expectedUser.MatchArg()))
                ;

            var sut = new AddUserRequestHandler(sessionFunc, query, guidGenerator);

            Guid? actualGuid = sut.HandleAddUserRequest(request);

            actualGuid.ShouldEqual(guid);

            guidGenerator.VerifyAllExpectations();
            query.VerifyAllExpectations();
            session.VerifyAllExpectations();
            sessionFunc.VerifyAllExpectations();

        }

        [Test]
        public void Test_UserAlreadyExists()
        {
            var session = MockRepository.GenerateMock<ISession>();
            var sessionFunc = MockRepository.GenerateMock<Func<ISession>>();
            var query = MockRepository.GenerateMock<UserExistsQuery>();
            var guidGenerator = MockRepository.GenerateMock<Func<Guid>>();

            Guid? guid = null;

            var request = new AddUserRequest()
            {
                EmailAddress = "email@example.org",
                UserName = "username",
            };

            query.Expect(q => q.CheckIfUserExists(request.MatchArg()))
                .Return(true)
                ;

            guidGenerator.AssertWasNotCalled(g => g());

            sessionFunc.AssertWasNotCalled(f => f());

            session.AssertWasNotCalled(s => s.Save(Arg<User>.Is.Anything));

            var sut = new AddUserRequestHandler(sessionFunc, query, guidGenerator);

            Guid? actualGuid = sut.HandleAddUserRequest(request);

            actualGuid.ShouldEqual(guid);

            guidGenerator.VerifyAllExpectations();
            query.VerifyAllExpectations();
            session.VerifyAllExpectations();
            sessionFunc.VerifyAllExpectations();

        }

    }
}
