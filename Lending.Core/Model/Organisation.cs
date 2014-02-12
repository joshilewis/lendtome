using System;
using System.Collections.Generic;

namespace Lending.Core.Model
{
    public class Organisation : IOwner
    {
        public virtual int Id { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual ISet<User> Administrators { get; protected set; }

        protected Organisation() { }

        public Organisation(string name)
        {
            Name = name;
            Administrators=new HashSet<User>();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Id.Equals(((Organisation) obj).Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
