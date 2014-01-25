using System;

namespace Lending.Core.Model
{
    public class Borrowing
    {
        public virtual Guid Id { get; protected set; }
        public virtual User Borrower { get; protected set; }
        public virtual Ownership Ownership { get; protected set; }
        public virtual DateTime RequestTime { get; protected set; }

        protected Borrowing() { }

        public Borrowing(User borrower, Ownership ownership)
        {
            Borrower = borrower;
            Ownership = ownership;
            RequestTime = DateTime.Now;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Id.Equals(((Borrowing) obj).Id);

        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
