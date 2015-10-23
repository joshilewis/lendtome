using System;
using System.Collections.Generic;
using Lending.Cqrs;
using Lending.Domain.AcceptConnection;
using Lending.Domain.AddBookToLibrary;
using Lending.Domain.RegisterUser;
using Lending.Domain.RemoveBookFromLibrary;
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
        public const string BookAlreadyInLibrary = "The user already has that book in his library";
        public const string BookNotInLibrary = "The user does not have that book in his library";

        public string UserName { get; protected set; }
        public string EmailAddress { get; protected set; }

        public List<Guid> PendingConnectionRequests { get; set; }
        public List<Guid> ReceivedConnectionRequests { get; set; }
        public List<Guid> ConnectedUsers { get; set; }
        public List<Book> Library { get; set; } 

        protected User(Guid processId, Guid id, string userName, string emailAddress)
            : this()
        {
            RaiseEvent(new UserRegistered(processId, id, userName, emailAddress));
        }

        protected User()
        {
            PendingConnectionRequests = new List<Guid>();
            ReceivedConnectionRequests = new List<Guid>();
            ConnectedUsers = new List<Guid>();
            Library = new List<Book>();
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

        protected virtual void When(ConnectionRequestReceived @event)
        {
            ReceivedConnectionRequests.Add(@event.RequestingUserId);
        }

        protected virtual void When(ConnectionAccepted @event)
        {
            ReceivedConnectionRequests.Remove(@event.RequestingUserId);
            ConnectedUsers.Add(@event.RequestingUserId);
        }

        protected virtual void When(ConnectionCompleted @event)
        {
            PendingConnectionRequests.Remove(@event.AcceptingUserId);
            ConnectedUsers.Add(@event.AcceptingUserId);
        }

        protected virtual void When(BookAddedToLibrary @event)
        {
            Library.Add(new Book(@event.Title, @event.Author, @event.Isbn));
        }

        protected virtual void When(BookRemovedFromLibrary @event)
        {
            Library.Remove(new Book(@event.Title, @event.Author, @event.Isbn));
        }

        protected override List<IEventRoute> EventRoutes => new List<IEventRoute>()
        {
            new EventRoute<UserRegistered>(When, typeof(UserRegistered)),
            new EventRoute<ConnectionRequested>(When, typeof(ConnectionRequested)),
            new EventRoute<ConnectionRequestReceived>(When, typeof(ConnectionRequestReceived)),
            new EventRoute<ConnectionAccepted>(When, typeof(ConnectionAccepted)),
            new EventRoute<ConnectionCompleted>(When, typeof(ConnectionCompleted)),
            new EventRoute<BookAddedToLibrary>(When, typeof(BookAddedToLibrary)),
            new EventRoute<BookRemovedFromLibrary>(When, typeof(BookRemovedFromLibrary)),
        };


        public Result RequestConnection(Guid processId, Guid destinationUserId)
        {
            if (PendingConnectionRequests.Contains(destinationUserId)) return new Result(ConnectionAlreadyRequested);
            if (ReceivedConnectionRequests.Contains(destinationUserId)) return new Result(ReverseConnectionAlreadyRequested);
            if(ConnectedUsers.Contains(destinationUserId)) return new Result(UsersAlreadyConnected);

            RaiseEvent(new ConnectionRequested(processId, Id, destinationUserId));
            return new Result();
        }

        public Result InitiateConnectionAcceptance(Guid processId, Guid sourceUserId)
        {
            if (ReceivedConnectionRequests.Contains(sourceUserId)) return Fail(ReverseConnectionAlreadyRequested);
            if (ConnectedUsers.Contains(sourceUserId)) return Fail(UsersAlreadyConnected);

            RaiseEvent(new ConnectionRequestReceived(processId, Id, sourceUserId));
            return Success();
        }

        public Result AcceptConnection(Guid processId, Guid requestingUserId)
        {
            if (ConnectedUsers.Contains(requestingUserId)) return Fail(UsersAlreadyConnected);
            if (!ReceivedConnectionRequests.Contains(requestingUserId)) return new Result(ConnectionRequestNotReceived);

            RaiseEvent(new ConnectionAccepted(processId, Id, requestingUserId));

            return Success();
        }

        public Result CompleteConnection(Guid processId, Guid acceptingUserId)
        {
            if (ConnectedUsers.Contains(acceptingUserId)) return Fail(UsersAlreadyConnected);
            if (!PendingConnectionRequests.Contains(acceptingUserId)) return Fail(ConnectionNotRequested);

            RaiseEvent(new ConnectionCompleted(processId, Id, acceptingUserId));

            return Success();
        }

        public Result AddBookToLibrary(Guid processId, string title, string author, string isbn)
        {
            if (Library.Contains(new Book(title, author, isbn))) return Fail(BookAlreadyInLibrary);
            RaiseEvent(new BookAddedToLibrary(processId, Id, title, author, isbn));
            return Success();
        }

        public Result RemoveBookFromLibrary(Guid processId, string title, string author, string isbn)
        {
            if (!Library.Contains(new Book(title, author, isbn))) return Fail(BookNotInLibrary);
            RaiseEvent(new BookRemovedFromLibrary(processId, Id, title, author, isbn));

            return Success();
        }
    }

    public class Book
    {
        public string Title { get; protected set; }
        public string Author { get; protected set; }
        public string Isbn { get; protected set; }

        public Book(string title, string author, string isbn)
        {
            Title = title;
            Author = author;
            Isbn = isbn;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Book)) return false;

            Book other = (Book) obj;

            return Title.Equals(other.Title) &&
                   Author.Equals(other.Author) &&
                   Isbn.Equals(other.Isbn);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
