using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace Lending.ReadModels.Relational.LinkRequested
{
    public class RequestLinkMap  :ClassMap<RequestedLink>
    {
        public RequestLinkMap()
        {
            Id(x => x.Id)
                .GeneratedBy.Native();

            Map(x => x.ProcessId);

            Map(x => x.TargetAdministratorId)
                .UniqueKey("UK_RequestingLibraryId_TargetLibraryId");
            Map(x => x.TargetAdministratorPicture)
                .UniqueKey("UK_RequestingLibraryId_TargetLibraryId");
            Map(x => x.TargetLibraryId)
                .UniqueKey("UK_RequestingLibraryId_TargetLibraryId");
            Map(x => x.TargetLibraryName)
                .UniqueKey("UK_RequestingLibraryId_TargetLibraryId");

            Map(x => x.RequestingAdministratorId)
                .UniqueKey("UK_RequestingLibraryId_TargetLibraryId");
            Map(x => x.RequestingAdministratorPicture)
                .UniqueKey("UK_RequestingLibraryId_TargetLibraryId");
            Map(x => x.RequestingLibraryId)
                .UniqueKey("UK_RequestingLibraryId_TargetLibraryId");
            Map(x => x.RequestingLibraryName)
                .UniqueKey("UK_RequestingLibraryId_TargetLibraryId");
        }
    }
}
