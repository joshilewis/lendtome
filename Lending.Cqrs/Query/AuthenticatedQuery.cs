using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lending.Cqrs.Query
{
    public abstract class AuthenticatedQuery : Query, IAuthenticated
    {
        public Guid UserId { get; set; }

        protected AuthenticatedQuery(Guid userId)
        {
            UserId = userId;
        }

        protected AuthenticatedQuery()
        {
        }
    }
}
