namespace Lending.Core.NewUser
{
    public class UserAddedEvent : Event
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }

        public UserAddedEvent(int id, string userName, string emailAddress)
        {
            Id = id;
            UserName = userName;
            EmailAddress = emailAddress;
        }

        protected UserAddedEvent()
        { }
    }
}