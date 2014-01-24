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

            Map(x => x.Title);
            Map(x => x.Creator);
            Map(x => x.Edition);

        }
    }
}
