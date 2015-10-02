using System;
using Lending.Core.NewUser;

namespace Lending.Core.Model
{
    public abstract class User : Aggregate
    {
        public virtual string UserName { get; protected set; }
        public virtual string EmailAddress { get; protected set; }

        protected User(string userName, string emailAddress)
        {
            RaiseEvent(new UserAdded(SequentialGuid.NewGuid(), userName, emailAddress));
        }

        protected User()
        {
        }

        protected virtual void When(UserAdded @event)
        {
            UserName = @event.UserName;
            EmailAddress = @event.EmailAddress;
        }
    }
}
