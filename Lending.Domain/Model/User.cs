using System.Collections.Generic;
using Lending.Domain.NewUser;

namespace Lending.Domain.Model
{
    public class User : Aggregate
    {
        public string UserName { get; protected set; }
        public string EmailAddress { get; protected set; }

        protected User(string userName, string emailAddress)
        {
            RaiseEvent(new UserAdded(SequentialGuid.NewGuid(), userName, emailAddress));
        }

        protected User()
        {
        }

        public static User Create(string userName, string emailAddress)
        {
            return new User(userName, emailAddress);
        }

        public static User CreateFromEvents(IEnumerable<Event> events)
        {
            var user = new User();
            foreach (var @event in events)
            {
                user.ApplyEvent(@event);
            }
            return user;
        }

        protected virtual void When(UserAdded @event)
        {
            UserName = @event.UserName;
            EmailAddress = @event.EmailAddress;
        }
    }
}
