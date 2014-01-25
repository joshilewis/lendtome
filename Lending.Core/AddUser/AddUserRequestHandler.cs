using System;
using Lending.Core.Model;
using NHibernate;

namespace Lending.Core.AddUser
{
    public class AddUserRequestHandler : IRequestHandler<AddUserRequest, BaseResponse>
    {
        public const string UsernameTaken = "That user name is already in use.";
        public const string EmailTaken = "That Email Address is already in use.";

        private readonly Func<ISession> getSession;

        public AddUserRequestHandler(Func<ISession> sessionFunc)
        {
            this.getSession = sessionFunc;
        }

        protected AddUserRequestHandler() { }

        public virtual BaseResponse HandleRequest(AddUserRequest request)
        {
            ISession session = getSession();

            if (UserWithUserNameExists(request))
                return new BaseResponse(UsernameTaken);

            if (UserWithEmailExists(request))
                return new BaseResponse(EmailTaken);

            var newUser = new User(request.UserName, request.EmailAddress);

            session.Save(newUser);

            return new BaseResponse();
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