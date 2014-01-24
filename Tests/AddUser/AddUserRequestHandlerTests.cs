using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.AddUser;
using Core.Model;
using Core.Model.Maps;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using Rhino.Mocks;

namespace Tests.AddUser
{
    [TestFixture]
    public class AddUserRequestHandlerTests : DatabaseFixtureBase
    {

        [Test]
        public void Test_Success()
        {
            //Arrange
            var request = new AddUserRequest() {EmailAddress = "test@example.org", UserName = "username"};
            var expectedResponse = new AddUserResponse() {Success = true};

            //Act
            var sut = new AddUserRequestHandler(() => Session);
            AddUserResponse actualResponse = sut.HandleRequest(request);

            //Assert
            actualResponse.ShouldEqual(expectedResponse);

            var expectedUser = new User(request.UserName, request.EmailAddress);

            CommitTransactionAndOpenNew();

            User userInDb = Session
                .QueryOver<User>()
                .SingleOrDefault()
                ;

            userInDb.ShouldEqual(expectedUser);
        }

        [Test]
        public void Test_Failed_UsernameTaken()
        {
            //Arrange
            var request = new AddUserRequest() { EmailAddress = "test@example.org", UserName = "username"};
            var expectedResponse = new AddUserResponse() { Success = false, FailureDescription = AddUserResponse.UsernameTaken };

            var existingUser = new User(request.UserName, "new email address");
            Session.Save(existingUser);

            CommitTransactionAndOpenNew();

            //Act
            var sut = new AddUserRequestHandler(() => Session);
            AddUserResponse actualResponse = sut.HandleRequest(request);

            //Assert
            actualResponse.ShouldEqual(expectedResponse);
        }

        [Test]
        public void Test_Failed_EmailAddressTaken()
        {
            //Arrange
            var request = new AddUserRequest() { EmailAddress = "test@example.org", UserName = "username" };
            var expectedResponse = new AddUserResponse() { Success = false, FailureDescription = AddUserResponse.EmailTaken };

            var existingUser = new User("new user name", request.EmailAddress);
            Session.Save(existingUser);

            CommitTransactionAndOpenNew();

            //Act
            var sut = new AddUserRequestHandler(() => Session);
            AddUserResponse actualResponse = sut.HandleRequest(request);

            //Assert
            actualResponse.ShouldEqual(expectedResponse);
        }

    }
}
