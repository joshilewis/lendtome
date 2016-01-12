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
            Map(x => x.RequestingLibraryId)
                .UniqueKey("UK_RequestingLibraryId_AcceptingLibraryId");
            Map(x => x.AcceptingLibraryId)
                .UniqueKey("UK_RequestingLibraryId_AcceptingLibraryId");
        }
    }
}