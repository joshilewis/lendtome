using System;

namespace Joshilewis.Cqrs
{
    public abstract class AuthenticatedMessage : Message, IAuthenticated
    {
        public Guid UserId { get; }

        protected AuthenticatedMessage(Guid userId)
        {
            UserId = userId;
        }
    }
}
