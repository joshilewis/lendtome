using FluentNHibernate.Mapping;
using Lending.Domain.RegisterUser;

namespace Lending.Execution.Persistence
{
    public class RegisteredUserMap : ClassMap<RegisteredUser>
    {
        public RegisteredUserMap()
        {
            Id(x => x.Id)
                .GeneratedBy.Assigned();
            Map(x => x.UserName);
            Map(x => x.AuthUserId)
                .Unique();
        }
    }
}
