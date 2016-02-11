using System;

namespace Joshilewis.Cqrs.Command
{
    public abstract class AuthenticatedCommand : Command, IAuthenticated
    {
        public Guid UserId { get; set; }

        protected AuthenticatedCommand(Guid processId, Guid aggregateId, Guid userId)
            : base(processId, aggregateId)
        {
            UserId = userId;
        }

        protected AuthenticatedCommand()
        {
        }
    }
}
