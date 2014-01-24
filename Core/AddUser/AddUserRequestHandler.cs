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

        public virtual AddUserResponse HandleAddUserRequest(AddUserRequest request)
        {
            ISession session = getSession();

            if (UserWithUserNameExists(request))
                return new AddUserResponse() {Success = false, FailureDescription = AddUserResponse.UsernameTaken};

            if (UserWithEmailExists(request))
                return new AddUserResponse() { Success = false, FailureDescription = AddUserResponse.EmailTaken };

            var newUser = new User(request.UserName, request.EmailAddress);

            session.Save(newUser);

            return new AddUserResponse() {Success = true};
        }

        private bool UserWithUserNameExists(AddUserRequest request)
        {
            return getSession()
                .QueryOver<User>()
                .Where(u => u.UserName == request.UserName)
                .RowCount() > 0
                ;
        }

        private bool UserWithEmailExists(AddUserRequest request)
        {
            return getSession()
                .QueryOver<User>()
                .Where(u => u.EmailAddress == request.EmailAddress)
                .RowCount() > 0
                ;
        }

    }
}