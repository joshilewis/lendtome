using FluentNHibernate.Mapping;

namespace Lending.Core.Model.Maps
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(x => x.Id)
                .GeneratedBy.Native();

        }
    }
}
