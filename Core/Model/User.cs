using System;

namespace Core.Model
{
    public class User : IOwner, IBorrower
    {
        public virtual Guid Id { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual string EmailAddress { get; protected set; }

        public User(Guid id, string name, string emailAddress)
        {
            this.Id = id;
            Name = name;
            EmailAddress = emailAddress;
        }

        protected User() { }
    }
}
