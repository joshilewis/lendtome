using System;

namespace Joshilewis.Cqrs
{
    public abstract class AuthenticatedMessage : Message, IAuthenticated
    {
        public string UserId { get; }

        protected AuthenticatedMessage(string userId)
        {
            UserId = userId;
        }
    }
}
