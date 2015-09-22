using System;

namespace Lending.Core.ConnectionRequest
{
    public class ConnectionRequested : Event
    {
        public long FromUserId { get; set; }
        public long ToUserId { get; set; }

        public ConnectionRequested(Guid id, long fromUserId, long toUserId)
            : base(id)
        {
            FromUserId = fromUserId;
            ToUserId = toUserId;
        }
    }
}