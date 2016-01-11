using System;

namespace Lending.Domain.RegisterUser
{
    public class RegisteredUser
    {
        public virtual Guid Id { get; protected set; }
        public virtual string UserName { get; protected set; }

        public RegisteredUser(Guid id, string userName)
        {
            Id = id;
            UserName = userName;
        }

        protected RegisteredUser()
        {
        }
    }
}
