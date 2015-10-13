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

        protected AuthenticatedCommand(Guid processId, Guid aggregateId, Guid userId)
            : base(processId, aggregateId)
        {
            UserId = userId;
        }
    }
}
