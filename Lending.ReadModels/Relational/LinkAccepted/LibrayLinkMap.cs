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

            Map(x => x.AcceptingAdministratorId)
                .UniqueKey("UK_RequestingLibraryId_AcceptingLibraryId");
            Map(x => x.AcceptingAdministratorPicture)
                .UniqueKey("UK_RequestingLibraryId_AcceptingLibraryId");
            Map(x => x.AcceptingLibraryId)
                .UniqueKey("UK_RequestingLibraryId_AcceptingLibraryId");
            Map(x => x.AcceptingLibraryName)
                .UniqueKey("UK_RequestingLibraryId_AcceptingLibraryId");

            Map(x => x.RequestingAdministratorId)
                .UniqueKey("UK_RequestingLibraryId_AcceptingLibraryId");
            Map(x => x.RequestingAdministratorPicture)
                .UniqueKey("UK_RequestingLibraryId_AcceptingLibraryId");
            Map(x => x.RequestingLibraryId)
                .UniqueKey("UK_RequestingLibraryId_AcceptingLibraryId");
            Map(x => x.RequestingLibraryName)
                .UniqueKey("UK_RequestingLibraryId_AcceptingLibraryId");

        }
    }
}