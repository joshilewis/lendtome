using FluentNHibernate.Mapping;

namespace Lending.ReadModels.Relational.ConnectionAccepted
{
    public class UserConnectionMap : ClassMap<UserConnection>
    {
        public UserConnectionMap()
        {
            Id(x => x.Id)
                .GeneratedBy.Native();

            Map(x => x.ProcessId);
            Map(x => x.RequestingUserId)
                .UniqueKey("UK_RequestingUserId_AcceptingUserId");
            Map(x => x.AcceptingUserId)
                .UniqueKey("UK_RequestingUserId_AcceptingUserId");
        }
    }
}