using System;

namespace Lending.Domain.RegisterUser
{
    public class UserRegistered : Event
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }

        public UserRegistered(Guid id, string userName, string emailAddress)
            : base(id)
        {
            UserName = userName;
            EmailAddress = emailAddress;
        }

        protected UserRegistered() { }

    }
}