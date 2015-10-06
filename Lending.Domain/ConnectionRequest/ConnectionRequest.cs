using System;

namespace Lending.Domain.ConnectionRequest
{
    public class ConnectionRequest
    {
        public Guid SourceUserId { get; set; }
        public Guid TargetUserId { get; set; }

        public ConnectionRequest()
        {
            
        }

        public ConnectionRequest(Guid sourceUserId, Guid targetUserId)
        {
            SourceUserId = sourceUserId;
            TargetUserId = targetUserId;
        }
    }
}