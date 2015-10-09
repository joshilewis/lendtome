using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace Lending.Domain.Persistence
{
    public class PendingConnectionRequestMap : ClassMap<PendingConnectionRequest>
    {
        public PendingConnectionRequestMap()
        {
            Id(x => x.SourceUserId).GeneratedBy.Assigned();
            Map(x => x.TargetUserId).Unique();
        }
    }
}
