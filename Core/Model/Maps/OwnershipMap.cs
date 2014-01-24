using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FluentNHibernate.Mapping;

namespace Core.Model.Maps
{
    public abstract class OwnershipMap<T> : BaseMap<Ownership<T>> where T : IOwner
    {
        public OwnershipMap()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            References(x => x.Item)
                .Column("ItemId")
                .Cascade.None()
                ;

        }
    }

    public class UserOwnershipMap : OwnershipMap<User>
    {
        public UserOwnershipMap()
        {
            References(x => x.Owner)
                .Column("OwnerId")
                .Cascade.None()
                ;
        }
    }

    public class OrganisationOwnershipMap : OwnershipMap<Organisation>
    {
        public OrganisationOwnershipMap()
        {
            References(x => x.Owner)
                .Column("OwnerId")
                .Cascade.None()
                ;
        }
    }

}
