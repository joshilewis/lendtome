using System;
using System.Collections.Generic;
using Lending.Domain.RegisterUser;
using Lending.Domain.RequestConnection;

namespace Lending.Domain.Model
{
    public class User : Aggregate
    {
        public const string ConnectionAlreadyRequested = "A connection request for these users already exists";
        public const string ReverseConnectionAlreadyRequested = "A reverse connection request for these users already exists";

        public string UserName { get; protected set; }
        public string EmailAddress { get; protected set; }

        public List<Guid> CurrentConnectionRequests { get; set; }
        public List<Guid> ReceivedConnectionRequests { get; set; }

        protected User(Guid processId, Guid id, string userName, string emailAddress)
            : this()
        {
            RaiseEvent(new UserRegistered(processId, id, userName, emailAddress));
        }

        protected User()
        {
            CurrentConnectionRequests = new List<Guid>();
            ReceivedConnectionRequests = new List<Guid>();
        }

        public static User Register(Guid processId, Guid id, string userName, string emailAddress)
        {
            return new User(processId, id, userName, emailAddress);
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

        protected virtual void When(UserRegistered @event)
        {
            Id = @event.AggregateId;
            UserName = @event.UserName;
            EmailAddress = @event.EmailAddress;
        }

        protected virtual void When(ConnectionRequested @event)
        {
            CurrentConnectionRequests.Add(@event.DesintationUserId);
        }

        protected virtual void When(ConnectionRequestReceived @event)
        {
            ReceivedConnectionRequests.Add(@event.SourceUserId);
        }

        protected override List<IEventRoute> EventRoutes => new List<IEventRoute>()
        {
            new EventRoute<UserRegistered>(When, typeof(UserRegistered)),
            new EventRoute<ConnectionRequested>(When, typeof(ConnectionRequested)),
            new EventRoute<ConnectionRequestReceived>(When, typeof(ConnectionRequestReceived)),
        };

        public Response RequestConnectionTo(Guid processId, Guid desintationUserId)
        {
            if (CurrentConnectionRequests.Contains(desintationUserId)) return new Response(ConnectionAlreadyRequested);
            if (ReceivedConnectionRequests.Contains(desintationUserId)) return new Response(ReverseConnectionAlreadyRequested);

            RaiseEvent(new ConnectionRequested(processId, Id, desintationUserId));
            return new Response();
        }

        public bool ReceiveConnectionRequest(Guid processId, Guid sourceUserId)
        {
            if (ReceivedConnectionRequests.Contains(sourceUserId)) return false;

            RaiseEvent(new ConnectionRequestReceived(processId, Id, sourceUserId));
            return true;
        }

    }
}
