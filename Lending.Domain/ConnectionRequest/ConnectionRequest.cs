using System;

namespace Lending.Domain.ConnectionRequest
{
    public class ConnectionRequest : AuthenticatedRequest
    {
        public Guid TargetUserId { get; set; }

        public ConnectionRequest()
        { }

        public ConnectionRequest(Guid sourceUserId, Guid targetUserId) 
            : base(sourceUserId)
        {
            TargetUserId = targetUserId;
        }
    }
}