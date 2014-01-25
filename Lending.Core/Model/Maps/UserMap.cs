namespace Lending.Core.Model.Maps
{
    public class UserMap : BaseMap<User>
    {
        public UserMap()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            Map(x => x.UserName);
            Map(x => x.EmailAddress);

        }
    }
}
