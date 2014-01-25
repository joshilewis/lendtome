using System;

namespace Lending.Core.Model
{
    public class Item
    {
        public virtual Guid Id { get; protected set; }
        public virtual string Title { get; protected set; }
        public virtual string Creator { get; protected set; }
        public virtual string Edition { get; protected set; }

        protected Item() { }

        public Item(string title, string creator, string edition)
        {
            Title = title;
            Creator = creator;
            Edition = edition;
        }

        public override bool Equals(object obj)
        {

            if (!(obj is Item))
                return false;

            return Id.Equals(((Item) obj).Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
