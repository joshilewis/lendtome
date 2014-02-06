using System;

namespace Lending.Core.Model
{
    public abstract class User : IBorrower, IOwner
    {
        public virtual Guid Id { get; protected set; }
        public abstract string UserName { get; }
        public abstract string EmailAddress { get; }

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
