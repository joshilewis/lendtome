using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace Lending.Domain.Persistence
{
    public class RegisteredUserMap : ClassMap<RegisteredUser>
    {
        public RegisteredUserMap()
        {
            Id(x => x.Id)
                .GeneratedBy.Assigned();
            Map(x => x.UserName);
        }
    }
}
