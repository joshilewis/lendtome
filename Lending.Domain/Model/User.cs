using System;
using System.Collections.Generic;
using Lending.Domain.AcceptConnection;
using Lending.Domain.RegisterUser;
using Lending.Domain.RequestConnection;

namespace Lending.Domain.Model
{
    public class User : Aggregate
    {
        public const string UsersAlreadyConnected = "This user is already connected to the specified user";
        public const string ConnectionRequestNotReceived = "This user did not receive a connection request from the specified user";
        public const string ConnectionAlreadyRequested = "A connection request for these users already exists";
        public const string ReverseConnectionAlreadyRequested = "A reverse connection request for these users already exists";
        public const string ConnectionNotRequested = "This user did not request a connection with the specified user";

        public string UserName { get; protected set; }
        public string EmailAddress { get; protected set; }

        public List<Guid> PendingConnectionRequests { get; set; }
        public List<Guid> ReceivedConnectionRequests { get; set; }
        public List<Guid> ConnectedUsers { get; set; }

        protected User(Guid processId, Guid id, string userName, string emailAddress)
            : this()
        {
            RaiseEvent(new UserRegistered(processId, id, userName, emailAddress));
        }

        protected User()
        {
            PendingConnectionRequests = new List<Guid>();
            ReceivedConnectionRequests = new List<Guid>();
            ConnectedUsers=new List<Guid>();
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
            PendingConnectionRequests.Add(@event.DesintationUserId);
        }

        protected virtual void When(ConnectionRequestReceived @event)
        {
            ReceivedConnectionRequests.Add(@event.SourceUserId);
        }

        protected virtual void When(ReceivedConnectionAccepted @event)
        {
            ReceivedConnectionRequests.Remove(@event.RequestingUserId);
            ConnectedUsers.Add(@event.RequestingUserId);
        }

        protected virtual void When(RequestedConnectionAccepted @event)
        {
            PendingConnectionRequests.Remove(@event.AcceptingUserId);
            ConnectedUsers.Add(@event.AcceptingUserId);
        }

        protected override List<IEventRoute> EventRoutes => new List<IEventRoute>()
        {
            new EventRoute<UserRegistered>(When, typeof(UserRegistered)),
            new EventRoute<ConnectionRequested>(When, typeof(ConnectionRequested)),
            new EventRoute<ConnectionRequestReceived>(When, typeof(ConnectionRequestReceived)),
            new EventRoute<ReceivedConnectionAccepted>(When, typeof(ReceivedConnectionAccepted)),
            new EventRoute<RequestedConnectionAccepted>(When, typeof(RequestedConnectionAccepted)),
        };

        public Response RequestConnectionTo(Guid processId, Guid desintationUserId)
        {
            if (PendingConnectionRequests.Contains(desintationUserId)) return new Response(ConnectionAlreadyRequested);
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

        public Response AcceptReceivedConnection(Guid processId, Guid requestingUserId)
        {
            if (ConnectedUsers.Contains(requestingUserId)) return new Response(UsersAlreadyConnected);
            if (!ReceivedConnectionRequests.Contains(requestingUserId)) return new Response(ConnectionRequestNotReceived);

            RaiseEvent(new ReceivedConnectionAccepted(processId, Id, requestingUserId));

            return new Response();
        }

        public Response NotifyConnectionAccepted(Guid processId, Guid acceptingUserId)
        {
            if (ConnectedUsers.Contains(acceptingUserId)) return new Response(UsersAlreadyConnected);
            if (!PendingConnectionRequests.Contains(acceptingUserId)) return new Response(ConnectionNotRequested);

            RaiseEvent(new RequestedConnectionAccepted(processId, Id, acceptingUserId));

            return new Response();
        }
    }
}
