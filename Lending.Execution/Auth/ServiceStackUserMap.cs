using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FluentNHibernate.Mapping;

namespace Lending.Execution.Auth
{
    public class ServiceStackUserMap : SubclassMap<ServiceStackUser>
    {
        public ServiceStackUserMap()
        {
            Map(x => x.AuthenticatedUserId)
                .Unique();
        }
    }
}
