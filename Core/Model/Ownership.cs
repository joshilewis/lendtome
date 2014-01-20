using System;

namespace Core.Model
{
    public class Ownership
    {
        public virtual Guid Id { get; protected set; }
        public virtual Item Item { get; protected set; }
        public virtual IOwner Owner { get; protected set; }

        protected Ownership() { }
    }
}
