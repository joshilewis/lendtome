using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace Lending.Execution.Auth
{
    public class AuthenticatedUserMap : ClassMap<AuthenticatedUser>
    {
        public AuthenticatedUserMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.UserName);
            Map(x => x.Email);
            Map(x => x.Picture);
            HasMany(x => x.AuthenticationProviders)
                .KeyColumn("AuthenticatedUserId")
                .Cascade.All();
        }
    }
}
