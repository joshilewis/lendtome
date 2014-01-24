using System;
using Core.Model;
using NHibernate;

namespace Core.AddUser
{
    public class AddUserRequestHandler
    {
        private readonly Func<ISession> getSession;

        public AddUserRequestHandler(Func<ISession> sessionFunc)
        {
            this.getSession = sessionFunc;
        }

        protected AddUserRequestHandler() { }

        public virtual Guid? HandleAddUserRequest(AddUserRequest request)
        {
            ISession session = getSession();

            bool userExists = session
                .QueryOver<User>()
                .Where(u => u.UserName == request.UserName || u.EmailAddress == request.EmailAddress)
                .RowCount() > 0
                ;

            if (userExists)
                return null;

            var newUser = new User(request.UserName, request.EmailAddress);

            session.Save(newUser);

            return newUser.Id;
        }
    }
}