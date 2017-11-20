using System;
using Joshilewis.Cqrs.Command;

namespace Lending.Domain.AcceptLink
{
    public class AcceptLink : AuthenticatedCommand
    {
        public Guid RequestingLibraryId { get; set; }

        public AcceptLink(Guid processId, Guid aggregateId, string userId, Guid requestingLibraryId)
            : base(processId, aggregateId, userId)
        {
            RequestingLibraryId = requestingLibraryId;
        }

        public AcceptLink()
        {
            
        }
    }
}