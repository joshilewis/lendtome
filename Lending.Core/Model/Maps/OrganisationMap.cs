using FluentNHibernate.Mapping;

namespace Lending.Core.Model.Maps
{
    public class OrganisationMap : ClassMap<Organisation>
    {
        public OrganisationMap()
        {
            Id(x => x.Id)
                .GeneratedBy.Native();

            Map(x => x.Name);

            //TODO: Map list of Administrators
        }
    }
}
