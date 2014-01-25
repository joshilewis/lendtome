namespace Lending.Core.Model.Maps
{
    public class ConnectionMap : BaseMap<Connection>
    {
        public ConnectionMap()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            References(x => x.User1)
                .Column("User1Id")
                .Cascade.None()
                ;

            References(x => x.User2)
                .Column("User2Id")
                .Cascade.None()
                ;

        }
    }
}
