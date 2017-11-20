using System;

namespace Joshilewis.Cqrs.Command
{
    public abstract class AuthenticatedCommand : Command, IAuthenticated
    {
        public string UserId { get; set; }

        protected AuthenticatedCommand(Guid processId, Guid aggregateId, string userId)
            : base(processId, aggregateId)
        {
            UserId = userId;
        }

        protected AuthenticatedCommand()
        {
        }
    }
}
