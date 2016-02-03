using FluentNHibernate.Mapping;

namespace Lending.ReadModels.Relational.LinkAccepted
{
    public class LibrayLinkMap : ClassMap<LibraryLink>
    {
        public LibrayLinkMap()
        {
            Id(x => x.Id)
                .GeneratedBy.Native();

            Map(x => x.ProcessId);
            References(x => x.RequestingLibrary)
                .Column("RequestingLibraryId")
                .UniqueKey("UK_RequestingLibraryId_AcceptingLibraryId");
            References(x => x.AcceptingLibrary)
                .Column("AcceptingLibraryId")
                .UniqueKey("UK_RequestingLibraryId_AcceptingLibraryId");
        }
    }
}