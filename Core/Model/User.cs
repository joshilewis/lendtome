using System;

namespace Core.Model
{
    public class User : IOwner, IBorrower
    {
        public virtual Guid Id { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual string EmailAddress { get; protected set; }

        public User(string name, string emailAddress)
        {
            Name = name;
            EmailAddress = emailAddress;
        }

        protected User() { }
    }
}
