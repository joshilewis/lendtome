using System;

namespace Lending.ReadModels.Relational.ConnectionAccepted
{
    public class UserConnection
    {
        public virtual long Id { get; protected set; }
        public virtual Guid ProcessId { get; protected set; }
        public virtual Guid RequestingUserId { get; protected set; }
        public virtual Guid AcceptingUserId { get; protected set; }

        public UserConnection(Guid processId, Guid requestingUserId, Guid acceptingUserId)
        {
            ProcessId = processId;
            RequestingUserId = requestingUserId;
            AcceptingUserId = acceptingUserId;
        }

        protected UserConnection(){ }
    }
}