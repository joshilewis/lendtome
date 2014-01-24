using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FluentNHibernate.Mapping;

namespace Core.Model.Maps
{
    public class ItemMapcs : BaseMap<Item>
    {
        public ItemMapcs()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            Map(x => x.Title)
                .UniqueKey("Unique_Item")
                ;
            Map(x => x.Creator)
                .UniqueKey("Unique_Item")
;
            Map(x => x.Edition)
                .UniqueKey("Unique_Item")
;

        }
    }
}
