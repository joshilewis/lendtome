using System;
using Lending.Cqrs.Command;

namespace Lending.Domain.AcceptLink
{
    public class AcceptLink : AuthenticatedCommand
    {
        public Guid RequestingLibraryId { get; set; }

        public AcceptLink(Guid processId, Guid aggregateId, Guid userId, Guid requestingLibraryId)
            : base(processId, aggregateId, userId)
        {
            RequestingLibraryId = requestingLibraryId;
        }

        public AcceptLink()
        {
            
        }
    }
}