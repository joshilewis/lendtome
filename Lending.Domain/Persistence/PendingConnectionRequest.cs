using System;

namespace Lending.Domain.Persistence
{
    public class PendingConnectionRequest
    {
        public PendingConnectionRequest(Guid sourceUserId, Guid targetUserId)
        {
            SourceUserId = sourceUserId;
            TargetUserId = targetUserId;
        }

        protected PendingConnectionRequest()
        {
            
        }

        public virtual Guid SourceUserId { get; protected set; }

        public virtual Guid TargetUserId { get; protected set; }

    }
}