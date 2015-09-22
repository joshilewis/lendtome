namespace Lending.Core.NewUser
{
    public class UserAdded : Event
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }

        public UserAdded(long id, string userName, string emailAddress)
            : base(id)
        {
            UserName = userName;
            EmailAddress = emailAddress;
        }

        protected UserAdded() { }

    }
}