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
            References(x => x.Library)
                .Column("LibraryId")
                .UniqueKey("UK_Library_Book");
            Map(x => x.LibraryName);
            Map(x => x.Title)
                .UniqueKey("UK_Library_Book");
            Map(x => x.Author)
                .UniqueKey("UK_Library_Book");
            Map(x => x.Isbn)
                .UniqueKey("UK_Library_Book");
        }
    }
}