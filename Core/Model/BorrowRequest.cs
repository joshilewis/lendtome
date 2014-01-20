using System;

namespace Core.Model
{
    public class BorrowRequest
    {
        public virtual Guid Id { get; protected set; }
        public virtual IBorrower Borrower { get; protected set; }
        public virtual Ownership Ownership { get; protected set; }
        public virtual DateTime RequestTime { get; protected set; }

        protected BorrowRequest() { }
    }
}
