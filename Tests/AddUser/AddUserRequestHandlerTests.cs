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
        public void Test_DoesntAlreadyExist()
        {
            ISession session = MockRepository.GenerateMock<ISession>();
            UserExistsQuery query = MockRepository.GenerateMock<UserExistsQuery>();
            Func<Guid> guidGenerator = MockRepository.GenerateMock<Func<Guid>>();



            var request = new AddUserRequest()
            {
                EmailAddress = "email@example.org",
                UserName = "username",
            };

            var expectedUser

        }
    }

    public class AddUserRequestHandler
    {
        private readonly Func<ISession> getSession;
        private readonly UserExistsQuery userExistsQuery;
        private readonly Func<Guid> getGuid;

        public AddUserRequestHandler(Func<ISession> sessionFunc,
            UserExistsQuery userExistsQuery,
            Func<Guid> guidFunc)
        {
            this.getSession = sessionFunc;
            this.userExistsQuery = userExistsQuery;
            this.getGuid = guidFunc;
        }

        protected AddUserRequestHandler() { }

        public virtual Guid? Handle(AddUserRequest request)
        {
            bool exists = userExistsQuery.CheckIfUserExists(request);

            if (exists)
                return null;

            var user = new User(getGuid(), request.UserName, request.EmailAddress);

            getSession().Save(user);

            return user.Id;
        }

    }

    public class UserExistsQuery
    {
        public virtual bool CheckIfUserExists(AddUserRequest request)
        {
            throw new NotImplementedException();
        }
    }

}
