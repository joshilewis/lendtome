using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Cqrs;

namespace Lending.Domain.AcceptConnection
{
    public class CompleteConnection: Command
    {
        public Guid AcceptingUserId { get; set; }

        public CompleteConnection(Guid processId, Guid aggregateId, Guid acceptingUserId) : base(processId, aggregateId)
        {
            AcceptingUserId = acceptingUserId;
        }
    }
}
