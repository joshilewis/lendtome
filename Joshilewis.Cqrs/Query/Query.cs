using System;

namespace Joshilewis.Cqrs.Query
{
    public class Query : Message
    {
        public Guid? UserId { get; set; }

        public Query(Guid? userId)
        {
            UserId = userId;
        }

        public Query()
        {
            UserId = null;
        }
    }
}