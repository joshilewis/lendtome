using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FluentNHibernate.Mapping;
using Lending.Core.Model.Maps;

namespace Lending.Execution.Auth
{
    public class ServiceStackUserMap : SubclassMap<ServiceStackUser>
    {
        public ServiceStackUserMap()
        {
            References(x => x.AuthenticatedUser)
                .Column("UserAuthId")
                .Cascade.All()
                ;

        }
    }
}
