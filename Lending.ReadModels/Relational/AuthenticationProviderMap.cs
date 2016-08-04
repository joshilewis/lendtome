using FluentNHibernate.Mapping;

namespace Lending.ReadModels.Relational
{
    public class AuthenticationProviderMap : ClassMap<AuthenticationProvider>
    {
        public AuthenticationProviderMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Name)
                .UniqueKey("AuthProvider_Name_Id");
            Map(x => x.UserId)
                .UniqueKey("AuthProvider_Name_Id");
        }
    }
}
