using System;
using System.Collections.Generic;
using Lending.Domain.NewUser;

namespace Lending.Domain.Model
{
    public class User : Aggregate
    {
        public string UserName { get; protected set; }
        public string EmailAddress { get; protected set; }

        protected User(Guid id, string userName, string emailAddress)
        {
            RaiseEvent(new UserAdded(id, userName, emailAddress));
        }

        protected User()
        {
        }

        public static User Create(Guid id, string userName, string emailAddress)
        {
            return new User(id, userName, emailAddress);
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
            Id = @event.Id;
            UserName = @event.UserName;
            EmailAddress = @event.EmailAddress;
        }

        protected override List<IEventRoute> EventRoutes => new List<IEventRoute>()
        {
            new EventRoute<UserAdded>(When, typeof(UserAdded)),
        };

        protected override string Type => "User";
    }
}
