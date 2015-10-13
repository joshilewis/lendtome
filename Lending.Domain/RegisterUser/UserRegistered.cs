using System;

namespace Lending.Domain.RegisterUser
{
    public class UserRegistered : Event
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }

        public UserRegistered(Guid processId, Guid aggregateId, string userName, string emailAddress)
            : base(processId, aggregateId)
        {
            UserName = userName;
            EmailAddress = emailAddress;
        }
    }
}