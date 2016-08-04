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
        public virtual OpenedLibrary RequestingLibrary { get; protected set; }
        public virtual OpenedLibrary TargetLibrary { get; protected set; }

        public RequestedLink(Guid processId, OpenedLibrary requestingLibrary, OpenedLibrary targetLibrary)
        {
            ProcessId = processId;
            RequestingLibrary = requestingLibrary;
            TargetLibrary = targetLibrary;
        }

        protected RequestedLink()
        {
        }

    }
}
