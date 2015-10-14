using System;
using System.Collections.Generic;
using Lending.Cqrs;
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
            PendingConnectionRequests.Add(@event.TargetUserId);
        }

        protected virtual void When(ConnectionApprovalInitiated @event)
        {
            ReceivedConnectionRequests.Add(@event.RequestingUserId);
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
            new EventRoute<ConnectionApprovalInitiated>(When, typeof(ConnectionApprovalInitiated)),
            new EventRoute<ReceivedConnectionAccepted>(When, typeof(ReceivedConnectionAccepted)),
            new EventRoute<RequestedConnectionAccepted>(When, typeof(RequestedConnectionAccepted)),
        };

        public Result RequestConnectionTo(Guid processId, Guid destinationUserId)
        {
            if (PendingConnectionRequests.Contains(destinationUserId)) return new Result(ConnectionAlreadyRequested);
            if (ReceivedConnectionRequests.Contains(destinationUserId)) return new Result(ReverseConnectionAlreadyRequested);
            if(ConnectedUsers.Contains(destinationUserId)) return new Result(UsersAlreadyConnected);

            RaiseEvent(new ConnectionRequested(processId, Id, destinationUserId));
            return new Result();
        }

        public Result InitiateConnectionApproval(Guid processId, Guid sourceUserId)
        {
            if (ReceivedConnectionRequests.Contains(sourceUserId)) return Fail(ReverseConnectionAlreadyRequested);
            if (ConnectedUsers.Contains(sourceUserId)) return Fail(UsersAlreadyConnected);

            RaiseEvent(new ConnectionApprovalInitiated(processId, Id, sourceUserId));
            return Success();
        }

        public Result AcceptReceivedConnection(Guid processId, Guid requestingUserId)
        {
            if (ConnectedUsers.Contains(requestingUserId)) return Fail(UsersAlreadyConnected);
            if (!ReceivedConnectionRequests.Contains(requestingUserId)) return new Result(ConnectionRequestNotReceived);

            RaiseEvent(new ReceivedConnectionAccepted(processId, Id, requestingUserId));

            return Success();
        }

        public Result NotifyConnectionAccepted(Guid processId, Guid acceptingUserId)
        {
            if (ConnectedUsers.Contains(acceptingUserId)) return Fail(UsersAlreadyConnected);
            if (!PendingConnectionRequests.Contains(acceptingUserId)) return Fail(ConnectionNotRequested);

            RaiseEvent(new RequestedConnectionAccepted(processId, Id, acceptingUserId));

            return Success();
        }
    }
}
