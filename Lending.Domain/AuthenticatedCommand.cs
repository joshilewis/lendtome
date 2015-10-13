using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lending.Domain
{
    public abstract class AuthenticatedCommand : Command
    {
        public Guid UserId { get; set; }

        protected AuthenticatedCommand(Guid aggregateId, Guid processId, Guid userId)
            : base(aggregateId, processId)
        {
            UserId = userId;
        }
    }
}
