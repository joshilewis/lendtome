using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace Lending.Execution.Auth
{
    public class AuthenticationProviderMap : ClassMap<AuthenticationProvider>
    {
        public AuthenticationProviderMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Name)
                .UniqueKey("AuthProvider_Name_Id");
            Map(x => x.UserId)
                .UniqueKey("AuthProvider_Name_Id");
        }
    }
}
