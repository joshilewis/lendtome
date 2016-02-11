using System;
using Joshilewis.Cqrs.Command;

namespace Lending.Domain.RequestLink
{
    public class RequestLink : AuthenticatedCommand
    {
        public Guid TargetLibraryId { get; set; }

        public RequestLink(Guid processId, Guid aggregateId, Guid userId, Guid targetLibraryId)
            : base(processId, aggregateId, userId)
        {
            TargetLibraryId = targetLibraryId;
        }

        public RequestLink()
        {
        }
    }
}