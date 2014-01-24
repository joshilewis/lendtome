using System;

namespace Core.Model
{
    public class User : IBorrower, IOwner
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

        public override bool Equals(object obj)
        {
            if (!(obj is User))
                return false;

            var other = (User) obj;
            return this.Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
