using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FluentNHibernate.Mapping;

namespace Core.Model.Maps
{
    public class UserMap : BaseMap<User>
    {
        public UserMap()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            Map(x => x.Name);
            Map(x => x.EmailAddress);

        }
    }
}
