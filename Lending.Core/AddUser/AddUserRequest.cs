namespace Lending.Core.AddUser
{
    public class AddUserRequest : Request
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
    }
}
