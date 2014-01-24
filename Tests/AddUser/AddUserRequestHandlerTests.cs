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
    public class AddUserRequestHandlerTests
    {
        private Configuration configuration;
        private ISessionFactory sessionFactory;
        private ISession session;
        private ITransaction transaction;

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            //Set up database
            configuration = Fluently.Configure()
                .Database(PostgreSQLConfiguration.PostgreSQL82
                    .ConnectionString(c => c.FromConnectionStringWithKey("lender_db")))
                .Mappings(m =>
                    m.FluentMappings
                        .AddFromAssemblyOf<UserMap>())
                .BuildConfiguration();

            sessionFactory = configuration.BuildSessionFactory();

        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
        }

        [SetUp]
        public void SetUp()
        {
            //Create DB
            new SchemaExport(configuration)
                .Execute(true, true, false);

            session = sessionFactory.OpenSession();
            transaction = session.BeginTransaction();

        }

        [TearDown]
        public void TearDown()
        {
            transaction.Commit();
            session.Flush();
            transaction.Dispose();
            session.Dispose();

            //Tear down DB
            new SchemaExport(configuration)
                .Execute(true, true, true);

        }

        [Test]
        public void Test_UserDoesntExist()
        {
            var request = new AddUserRequest() {EmailAddress = "test@example.org", UserName = "username"};

            var sut = new AddUserRequestHandler();

            Guid? userId = sut.HandleAddUserRequest(request);

            Assert.That(userId, Is.Not.Null);

            var expectedUser = new User(request.UserName, request.EmailAddress);

            session.Flush();

            User userInDb = session
                .QueryOver<User>()
                .SingleOrDefault()
                ;

            userInDb.ShouldEqual(expectedUser, userId.Value);
        }

        [Test]
        public void Test_UserAlreadyExists()
        {

        }

    }

    public class AddUserRequestHandler
    {
        public virtual Guid? HandleAddUserRequest(AddUserRequest request)
        {
            return null;
        }
    }
}
