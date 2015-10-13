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
        public Guid TargetUserId { get; set; }

        public ConnectionRequestReceived(Guid id, Guid sourceUserId, Guid targetUserId) : base(id)
        {
            SourceUserId = sourceUserId;
            TargetUserId = targetUserId;
        }
    }
}
