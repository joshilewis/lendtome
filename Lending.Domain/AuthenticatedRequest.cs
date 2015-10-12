using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lending.Domain
{
    public abstract class AuthenticatedRequest : Request
    {
        public Guid UserId { get; set; }

        protected AuthenticatedRequest() { }

        protected AuthenticatedRequest(Guid userId)
        {
            UserId = userId;
        }
    }
}
