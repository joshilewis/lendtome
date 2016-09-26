using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lending.ReadModels.Relational.LibraryOpened;

namespace Lending.ReadModels.Relational.LinkRequested
{
    public class RequestedLink
    {
        public virtual long Id { get; protected set; }
        public virtual Guid ProcessId { get; protected set; }
        public virtual Guid RequestingLibraryId { get; protected set; }
        public virtual string RequestingLibraryName { get; protected set; }
        public virtual Guid RequestingAdministratorId { get; protected set; }
        public virtual string RequestingAdministratorPicture { get; protected set; }
        public virtual Guid TargetLibraryId { get; protected set; }
        public virtual string TargetLibraryName { get; protected set; }
        public virtual Guid TargetAdministratorId { get; protected set; }
        public virtual string TargetAdministratorPicture { get; protected set; }

        public RequestedLink(Guid processId, OpenedLibrary requestingLibrary, OpenedLibrary targetLibrary)
        {
            ProcessId = processId;
            RequestingLibraryId = requestingLibrary.Id;
            RequestingLibraryName = requestingLibrary.Name;
            RequestingAdministratorId = requestingLibrary.AdministratorId;
            RequestingAdministratorPicture = requestingLibrary.AdministratorPicture;
            TargetLibraryId = targetLibrary.Id;
            TargetLibraryName = targetLibrary.Name;
            TargetAdministratorId = targetLibrary.AdministratorId;
            TargetAdministratorPicture = targetLibrary.AdministratorPicture;
        }

        protected RequestedLink()
        {
        }

    }
}
