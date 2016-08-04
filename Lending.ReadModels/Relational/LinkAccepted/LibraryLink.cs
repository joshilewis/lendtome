using System;
using System.Collections.Generic;
using Lending.ReadModels.Relational.LibraryOpened;

namespace Lending.ReadModels.Relational.LinkAccepted
{
    public class LibraryLink
    {
        public virtual long Id { get; protected set; }
        public virtual Guid ProcessId { get; protected set; }
        public virtual OpenedLibrary RequestingLibrary { get; protected set; }
        public virtual OpenedLibrary AcceptingLibrary { get; protected set; }

        public LibraryLink(Guid processId, OpenedLibrary requestingLibrary, OpenedLibrary acceptingLibrary)
        {
            ProcessId = processId;
            RequestingLibrary = requestingLibrary;
            AcceptingLibrary = acceptingLibrary;
        }

        protected LibraryLink(){ }

    }
}