using System;
using Lending.Cqrs;

namespace Lending.Domain.RequestLink
{
    public class LinkRequestReceived : Event
    {
        public Guid RequestingLibraryId { get; set; }

        public LinkRequestReceived(Guid processId, Guid aggregateId, Guid requestingLibraryId)
            : base(processId, aggregateId)
        {
            RequestingLibraryId = requestingLibraryId;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            if (!base.Equals(obj)) return false;
            var other = (LinkRequestReceived)obj;
            return RequestingLibraryId.Equals(other.RequestingLibraryId);

        }

    }
}
