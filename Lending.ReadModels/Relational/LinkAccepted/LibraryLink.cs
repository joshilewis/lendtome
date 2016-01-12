using System;

namespace Lending.ReadModels.Relational.LinkAccepted
{
    public class LibraryLink
    {
        public virtual long Id { get; protected set; }
        public virtual Guid ProcessId { get; protected set; }
        public virtual Guid RequestingLibraryId { get; protected set; }
        public virtual Guid AcceptingLibraryId { get; protected set; }

        public LibraryLink(Guid processId, Guid requestingLibraryId, Guid acceptingLibraryId)
        {
            ProcessId = processId;
            RequestingLibraryId = requestingLibraryId;
            AcceptingLibraryId = acceptingLibraryId;
        }

        protected LibraryLink(){ }
    }
}