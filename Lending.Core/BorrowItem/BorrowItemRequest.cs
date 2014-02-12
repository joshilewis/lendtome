using System;

namespace Lending.Core.BorrowItem
{
    public class BorrowItemRequest
    {
        public int RequestorId { get; set; }
        public Guid OwnershipId { get; set; }
    }
}