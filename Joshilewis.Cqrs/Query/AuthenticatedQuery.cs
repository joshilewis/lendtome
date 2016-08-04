using System;

namespace Joshilewis.Cqrs.Query
{
    public abstract class AuthenticatedQuery : Query, IAuthenticated
    {
        public new Guid UserId { get; set; }

        protected AuthenticatedQuery(Guid userId)
        {
            UserId = userId;
        }

        protected AuthenticatedQuery()
        {
        }
    }
}
