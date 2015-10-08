using System;
using System.Collections.Generic;
using Lending.Domain.ConnectionRequest;
using Lending.Domain.NewUser;

namespace Lending.Domain.Model
{
    public class User : Aggregate
    {
        public string UserName { get; protected set; }
        public string EmailAddress { get; protected set; }

        public List<Guid> CurrentConnectionRequests { get; set; }

        protected User(Guid id, string userName, string emailAddress)
            : this()
        {
            RaiseEvent(new UserAdded(id, userName, emailAddress));
        }

        protected User()
        {
            CurrentConnectionRequests = new List<Guid>();
        }

        public static User Create(Guid id, string userName, string emailAddress)
        {
            return new User(id, userName, emailAddress);
        }

        public static User CreateFromHistory(IEnumerable<Event> events)
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
            new EventRoute<ConnectionRequested>(When, typeof(ConnectionRequested)),
        };

        public bool RequestConnectionTo(Guid toUserId)
        {
            if (CurrentConnectionRequests.Contains(toUserId)) return false;

            RaiseEvent(new ConnectionRequested(Guid.Empty, Id, toUserId));
            return true;
        }

        protected virtual void When(ConnectionRequested @event)
        {
            CurrentConnectionRequests.Add(@event.ToUserId);
        }
    }
}
