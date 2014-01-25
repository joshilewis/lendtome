namespace Lending.Core.Model.Maps
{
    public class OrganisationMap : BaseMap<Organisation>
    {
        public OrganisationMap()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            Map(x => x.Name);

            //TODO: Map list of Administrators
        }
    }
}
