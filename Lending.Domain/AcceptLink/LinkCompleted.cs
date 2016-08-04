using System;
using Joshilewis.Cqrs;

namespace Lending.Domain.AcceptLink
{
    public class LinkCompleted : Event
    {
        public Guid AcceptingLibraryId { get; set; }

        public LinkCompleted(Guid processId, Guid aggregateId, Guid acceptingLibraryId)
            : base(processId, aggregateId)
        {
            AcceptingLibraryId = acceptingLibraryId;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            if (!base.Equals(obj)) return false;
            var other = (LinkCompleted)obj;
            return AcceptingLibraryId.Equals(other.AcceptingLibraryId);

        }

        public override int GetHashCode()
        {
            return (base.GetHashCode() * 397) ^ AcceptingLibraryId.GetHashCode();
        }
    }
}