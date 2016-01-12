using System;
using Lending.Cqrs.Command;

namespace Lending.Domain.RequestLink
{
    public class RequestLink : AuthenticatedCommand
    {
        public Guid TargetLibraryId { get; set; }

        public RequestLink(Guid processId, Guid aggregateId, Guid libraryId, Guid targetLibraryId)
            : base(processId, aggregateId, libraryId)
        {
            TargetLibraryId = targetLibraryId;
        }

        public RequestLink()
        {
        }
    }
}