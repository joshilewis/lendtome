using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Lending.Core;
using Lending.Core.AddUser;
using Lending.Core.Model;
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
            var expectedResponse = new BaseResponse() {Success = true};

            //Act
            var sut = new AddUserRequestHandler(() => Session);
            BaseResponse actualResponseBase = sut.HandleRequest(request);

            //Assert
            actualResponseBase.ShouldEqual(expectedResponse);

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
            var expectedResponse = new BaseResponse() { Success = false, FailureDescription = AddUserRequestHandler.UsernameTaken };

            var existingUser = new User(request.UserName, "new email address");
            Session.Save(existingUser);

            CommitTransactionAndOpenNew();

            //Act
            var sut = new AddUserRequestHandler(() => Session);
            BaseResponse actualResponseBase = sut.HandleRequest(request);

            //Assert
            actualResponseBase.ShouldEqual(expectedResponse);
        }

        [Test]
        public void Test_Failed_EmailAddressTaken()
        {
            //Arrange
            var request = new AddUserRequest() { EmailAddress = "test@example.org", UserName = "username" };
            var expectedResponse = new BaseResponse() { Success = false, FailureDescription = AddUserRequestHandler.EmailTaken };

            var existingUser = new User("new user name", request.EmailAddress);
            Session.Save(existingUser);

            CommitTransactionAndOpenNew();

            //Act
            var sut = new AddUserRequestHandler(() => Session);
            BaseResponse actualResponseBase = sut.HandleRequest(request);

            //Assert
            actualResponseBase.ShouldEqual(expectedResponse);
        }

    }
}
