using FluentNHibernate.Mapping;

namespace Lending.Core.Model.Maps
{
    public class ItemMapcs : ClassMap<Item>
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
