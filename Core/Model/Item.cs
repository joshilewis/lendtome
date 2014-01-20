using System;

namespace Core.Model
{
    public class Item
    {
        public virtual Guid Id { get; protected set; }
        public virtual string Title { get; protected set; }
        public virtual string Creator { get; protected set; }
        public virtual string Edition { get; protected set; }

        protected Item() { }
    }
}
