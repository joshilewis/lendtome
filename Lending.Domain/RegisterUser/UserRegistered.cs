using System;
using Lending.Cqrs;

namespace Lending.Domain.RegisterUser
{
    public class UserRegistered : Event
    {
        public long AuthUserId { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }

        public UserRegistered(Guid processId, Guid aggregateId, long authUserId, string userName, string emailAddress)
            : base(processId, aggregateId)
        {
            AuthUserId = authUserId;
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
            return AuthUserId.Equals(other.AuthUserId) &&
                UserName.Equals(other.UserName) &&
                   EmailAddress.Equals(other.EmailAddress);
        }

    }
}