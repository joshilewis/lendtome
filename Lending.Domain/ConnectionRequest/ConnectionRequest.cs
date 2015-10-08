using System;

namespace Lending.Domain.ConnectionRequest
{
    public class ConnectionRequest
    {
        public Guid TargetUserId { get; set; }

        public ConnectionRequest()
        { }

        public ConnectionRequest(Guid targetUserId)
        {
            TargetUserId = targetUserId;
        }
    }
}