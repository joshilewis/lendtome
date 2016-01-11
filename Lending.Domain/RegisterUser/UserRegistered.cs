using System;
using Lending.Cqrs;

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

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            if (!base.Equals(obj)) return false;
            var other = (UserRegistered)obj;
            return UserName.Equals(other.UserName) &&
                   EmailAddress.Equals(other.EmailAddress);
        }

    }
}