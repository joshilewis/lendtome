using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lending.Cqrs
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
