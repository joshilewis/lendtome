using System;

namespace Lending.Cqrs
{
    public abstract class AuthenticatedCommand : Command
    {
        public Guid UserId { get; set; }

        protected AuthenticatedCommand(Guid processId, Guid aggregateId, Guid userId)
            : base(processId, aggregateId)
        {
            UserId = userId;
        }
    }
}
