using FluentNHibernate.Mapping;
using Lending.Domain.OpenLibrary;

namespace Lending.ReadModels.Relational.LibraryOpened
{
    public class OpenedLibraryMap : ClassMap<OpenedLibrary>
    {
        public OpenedLibraryMap()
        {
            Id(x => x.Id)
                .GeneratedBy.Assigned();
            Map(x => x.Name);
            Map(x => x.AdministratorId);
        }
    }
}
