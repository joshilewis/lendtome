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
        public void Test_UserDoesntExist()
        {
            var request = new AddUserRequest() {EmailAddress = "test@example.org", UserName = "username"};

            var sut = new AddUserRequestHandler(() => Session);

            Guid? userId = sut.HandleAddUserRequest(request);

            Assert.That(userId, Is.Not.Null);

            var expectedUser = new User(request.UserName, request.EmailAddress);

            CommitTransactionAndOpenNew();

            User userInDb = Session
                .QueryOver<User>()
                .SingleOrDefault()
                ;

            userInDb.ShouldEqual(expectedUser, userId.Value);
        }

        [Test]
        public void Test_UserAlreadyExists()
        {
            var request = new AddUserRequest() { EmailAddress = "test@example.org", UserName = "username" };

            var existingUser = new User(request.UserName, request.EmailAddress);
            Session.Save(existingUser);

            CommitTransactionAndOpenNew();

            var sut = new AddUserRequestHandler(() => Session);

            Guid? userId = sut.HandleAddUserRequest(request);

            Assert.That(userId, Is.Null);
        }

    }
}
