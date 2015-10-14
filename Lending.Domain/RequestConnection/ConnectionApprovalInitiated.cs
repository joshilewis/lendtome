using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.Cqrs;

namespace Lending.Domain.RequestConnection
{
    public class ConnectionApprovalInitiated : Event
    {
        public Guid RequestingUserId { get; set; }

        public ConnectionApprovalInitiated(Guid processId, Guid aggregateId, Guid requestingUserId)
            : base(processId, aggregateId)
        {
            RequestingUserId = requestingUserId;
        }
    }
}
