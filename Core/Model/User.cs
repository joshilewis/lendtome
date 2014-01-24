using System;

namespace Core.Model
{
    public class User : IOwner, IBorrower
    {
        public virtual Guid Id { get; protected set; }
        public virtual string UserName { get; protected set; }
        public virtual string EmailAddress { get; protected set; }

        public User(string userName, string emailAddress)
        {
            UserName = userName;
            EmailAddress = emailAddress;
        }

        protected User() { }
    }
}
