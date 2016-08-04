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
            References(x => x.RequestingLibrary)
                .Column("RequestingLibraryId")
                .UniqueKey("UK_RequestingLibraryId_TargetLibraryId");
            References(x => x.TargetLibrary)
                .Column("TargetLibraryId")
                .UniqueKey("UK_RequestingLibraryId_TargetLibraryId");
        }
    }
}
