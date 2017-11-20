using System;

namespace Joshilewis.Cqrs.Query
{
    public class Query : Message
    {
        public string UserId { get; set; }

        public Query(string userId)
        {
            UserId = userId;
        }

        public Query()
        {
            UserId = null;
        }
    }
}