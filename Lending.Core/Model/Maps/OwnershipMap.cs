using System.Configuration;
using FluentNHibernate.Mapping;

namespace Lending.Core.Model.Maps
{
    public class OwnershipMap : BaseMap<Ownership>
    {
        public OwnershipMap()
        {
            string schema = ConfigurationManager.AppSettings["lender_db_schema"];
            Schema(schema);

            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            References(x => x.Item)
                .Column("ItemId")
                .Cascade.None()
                ;

        }
    }

    public abstract class OwnershipMap<T> : SubclassMap<Ownership<T>> where T : IOwner
    {
        public OwnershipMap()
        {
            string schema = ConfigurationManager.AppSettings["lender_db_schema"];
            Schema(schema);

            KeyColumn("OwnershipId");

            Table(TableName);

            References(x => x.Owner)
                .Column(OwnerTypeColumnName)
                .LazyLoad()
                .Cascade.None()
                ;

        }

        protected abstract string TableName { get; }

        protected abstract string OwnerTypeColumnName { get; }
    }

    public class UserOwnershipMap : OwnershipMap<User>
    {
        protected override string TableName
        {
            get { return "UserOwnership"; }
        }

        protected override string OwnerTypeColumnName
        {
            get { return "OwnerId"; }
        }
    }

    public class OrganisationOwnershipMap : OwnershipMap<Organisation>
    {
        protected override string TableName
        {
            get { return "OrganisationOwnership"; }
        }

        protected override string OwnerTypeColumnName
        {
            get { return "OrganisationId"; }
        }
    }

}
