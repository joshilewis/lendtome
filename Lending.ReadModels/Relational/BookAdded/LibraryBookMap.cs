using FluentNHibernate.Mapping;

namespace Lending.ReadModels.Relational.BookAdded
{
    public class LibraryBookMap : ClassMap<LibraryBook>
    {
        public LibraryBookMap()
        {
            Id(x => x.Id)
                .GeneratedBy.Native();

            Map(x => x.ProcessId);
            Map(x => x.OwnerId)
                .UniqueKey("UK_Owner_Book");
            Map(x => x.Title)
                .UniqueKey("UK_Owner_Book");
            Map(x => x.Author)
                .UniqueKey("UK_Owner_Book");
            Map(x => x.Isbn)
                .UniqueKey("UK_Owner_Book");
        }
    }
}