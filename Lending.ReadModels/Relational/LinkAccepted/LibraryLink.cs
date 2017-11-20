using System;
using System.Collections.Generic;
using Lending.ReadModels.Relational.LibraryOpened;

namespace Lending.ReadModels.Relational.LinkAccepted
{
    public class LibraryLink
    {
        public virtual long Id { get; protected set; }
        public virtual Guid ProcessId { get; protected set; }
        public virtual Guid RequestingLibraryId { get; protected set; }
        public virtual string RequestingLibraryName { get; protected set; }
        public virtual string RequestingAdministratorId { get; protected set; }
        public virtual string RequestingAdministratorPicture { get; protected set; }
        public virtual Guid AcceptingLibraryId { get; protected set; }
        public virtual string AcceptingLibraryName { get; protected set; }
        public virtual string AcceptingAdministratorId { get; protected set; }
        public virtual string AcceptingAdministratorPicture { get; protected set; }

        public LibraryLink(Guid processId, OpenedLibrary requestingLibrary, OpenedLibrary acceptingLibrary)
        {
            ProcessId = processId;
            RequestingLibraryId = requestingLibrary.Id;
            RequestingLibraryName = requestingLibrary.Name;
            RequestingAdministratorId = requestingLibrary.AdministratorId;
            RequestingAdministratorPicture = requestingLibrary.AdministratorPicture;
            AcceptingLibraryId = acceptingLibrary.Id;
            AcceptingLibraryName = acceptingLibrary.Name;
            AcceptingAdministratorId = acceptingLibrary.AdministratorId;
            AcceptingAdministratorPicture = acceptingLibrary.AdministratorPicture;
        }

        protected LibraryLink(){ }

    }
}