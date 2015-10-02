using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FluentNHibernate.Mapping;

namespace Lending.Execution.Auth
{
    public class ServiceStackUserMap : ClassMap<ServiceStackUser>
    {
        public ServiceStackUserMap()
        {
            Id(x => x.AuthenticatedUserId)
                .GeneratedBy.Assigned()
                ;

            Map(x => x.UserId);
        }
    }
}
