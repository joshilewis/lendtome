using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lending.Domain.RequestConnection
{
    public class ConnectionRequestReceived : Event
    {
        public Guid SourceUserId { get; set; }

        public ConnectionRequestReceived(Guid processId, Guid aggregateId, Guid sourceUserId)
            : base(processId, aggregateId)
        {
            SourceUserId = sourceUserId;
        }
    }
}
