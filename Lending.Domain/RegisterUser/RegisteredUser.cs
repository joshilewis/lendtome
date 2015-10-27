using System;

namespace Lending.Domain.RegisterUser
{
    public class RegisteredUser
    {
        public virtual long AuthUserId { get; set; }
        public virtual Guid Id { get; protected set; }
        public virtual string UserName { get; protected set; }

        public RegisteredUser(long authUserId, Guid id, string userName)
        {
            AuthUserId = authUserId;
            Id = id;
            UserName = userName;
        }

        protected RegisteredUser()
        {
        }
    }
}
