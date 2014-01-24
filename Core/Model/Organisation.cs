using System;
using System.Collections.Generic;

namespace Core.Model
{
    public class Organisation : IOwner
    {
        public virtual Guid Id { get; protected set; }
        public virtual string UserName { get; protected set; }
        public virtual List<User> Administrators { get; protected set; }

        protected Organisation() { }
    }
}
