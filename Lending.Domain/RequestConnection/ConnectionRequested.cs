using System;

namespace Lending.Domain.RequestConnection
{
    public class ConnectionRequested : Event
    {
        public Guid FromUserId { get; set; }
        public Guid ToUserId { get; set; }

        public ConnectionRequested(Guid id, Guid fromUserId, Guid toUserId)
            : base(id)
        {
            FromUserId = fromUserId;
            ToUserId = toUserId;
        }
    }
}