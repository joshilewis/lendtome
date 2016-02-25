using System;
using System.Collections.Generic;
using Joshilewis.Cqrs;
using Joshilewis.Cqrs.Exceptions;
using Joshilewis.Cqrs.Query;
using Lending.Domain.AcceptLink;
using Lending.Domain.AddBookToLibrary;
using Lending.Domain.OpenLibrary;
using Lending.Domain.RemoveBookFromLibrary;
using Lending.Domain.RequestLink;

namespace Lending.Domain.Model
{
    public class Library : Aggregate
    {
        public const string LibrariesAlreadyLinked = "This library is already linked to the specified library";
        public const string NoPendingLinkRequest = "This library did not receive a link request from the specified library";
        public const string LinkAlreadyRequested = "A link request for these libraries already exists";
        public const string ReverseLinkAlreadyRequested = "A reverse link request for these libraries already exists";
        public const string LinkNotRequested = "This library did not request to link with the specified library";
        public const string BookAlreadyInLibrary = "This book already exists in this library";
        public const string BookNotInLibrary = "This book is not in this library";

        public string Name { get; protected set; }
        public List<Guid> Administrators { get; private set; }

        public List<Guid> PendingLinkRequests { get; private set; }
        public List<Guid> ReceivedLinkRequests { get; private set; }
        public List<Guid> LinkedLibraries { get; private set; }
        public List<Book> Books { get; private set; } 

        protected Library(Guid processId, Guid id, string name, Guid adminId)
            : this()
        {
            RaiseEvent(new LibraryOpened(processId, id, name, adminId));
        }

        protected Library()
        {
            Administrators = new List<Guid>();
            PendingLinkRequests = new List<Guid>();
            ReceivedLinkRequests = new List<Guid>();
            LinkedLibraries = new List<Guid>();
            Books = new List<Book>();
        }

        public static Library Open(Guid processId, Guid id, string name, Guid adminId)
        {
            return new Library(processId, id, name, adminId);
        }

        public static Library CreateFromHistory(IEnumerable<Event> events)
        {
            var user = new Library();
            foreach (var @event in events)
            {
                user.ApplyEvent(@event);
            }
            return user;
        }

        protected virtual void When(LibraryOpened @event)
        {
            Id = @event.AggregateId;
            Name = @event.Name;
            Administrators.Add(@event.AdministratorId);
        }

        protected virtual void When(LinkRequested @event)
        {
            PendingLinkRequests.Add(@event.TargetLibraryId);
        }

        protected virtual void When(LinkRequestReceived @event)
        {
            ReceivedLinkRequests.Add(@event.RequestingLibraryId);
        }

        protected virtual void When(LinkAccepted @event)
        {
            ReceivedLinkRequests.Remove(@event.RequestingLibraryId);
            LinkedLibraries.Add(@event.RequestingLibraryId);
        }

        protected virtual void When(LinkCompleted @event)
        {
            PendingLinkRequests.Remove(@event.AcceptingLibraryId);
            LinkedLibraries.Add(@event.AcceptingLibraryId);
        }

        protected virtual void When(BookAddedToLibrary @event)
        {
            Books.Add(new Book(@event.Title, @event.Author, @event.Isbn));
        }

        protected virtual void When(BookRemovedFromLibrary @event)
        {
            Books.Remove(new Book(@event.Title, @event.Author, @event.Isbn));
        }

        protected override List<IEventRoute> EventRoutes => new List<IEventRoute>()
        {
            new EventRoute<LibraryOpened>(When, typeof(LibraryOpened)),
            new EventRoute<LinkRequested>(When, typeof(LinkRequested)),
            new EventRoute<LinkRequestReceived>(When, typeof(LinkRequestReceived)),
            new EventRoute<LinkAccepted>(When, typeof(LinkAccepted)),
            new EventRoute<LinkCompleted>(When, typeof(LinkCompleted)),
            new EventRoute<BookAddedToLibrary>(When, typeof(BookAddedToLibrary)),
            new EventRoute<BookRemovedFromLibrary>(When, typeof(BookRemovedFromLibrary)),
        };

        public void CheckUserAuthorized(Guid userId)
        {
            if (!Administrators.Contains(userId)) Fail(userId);
        }

        protected virtual void Fail(Guid userId)
        {
            throw new NotAuthorizedException(userId, Id, GetType());
        }

        public void RequestLink(Guid processId, Guid desinationLibraryId)
        {
            if (PendingLinkRequests.Contains(desinationLibraryId)) Fail(LinkAlreadyRequested);
            if (ReceivedLinkRequests.Contains(desinationLibraryId)) Fail(ReverseLinkAlreadyRequested);
            if(LinkedLibraries.Contains(desinationLibraryId)) Fail(LibrariesAlreadyLinked);

            RaiseEvent(new LinkRequested(processId, Id, desinationLibraryId));
        }

        public void InitiateLinkAcceptance(Guid processId, Guid sourceLibraryId)
        {
            if (ReceivedLinkRequests.Contains(sourceLibraryId)) Fail(ReverseLinkAlreadyRequested);
            if (LinkedLibraries.Contains(sourceLibraryId)) Fail(LibrariesAlreadyLinked);

            RaiseEvent(new LinkRequestReceived(processId, Id, sourceLibraryId));
        }

        public void AcceptLink(Guid processId, Guid requestingLibraryId)
        {
            if (LinkedLibraries.Contains(requestingLibraryId)) Fail(LibrariesAlreadyLinked);
            if (!ReceivedLinkRequests.Contains(requestingLibraryId)) Fail(NoPendingLinkRequest);

            RaiseEvent(new LinkAccepted(processId, Id, requestingLibraryId));
        }

        public void CompleteLink(Guid processId, Guid acceptingLibraryId)
        {
            if (LinkedLibraries.Contains(acceptingLibraryId)) Fail(LibrariesAlreadyLinked);
            if (!PendingLinkRequests.Contains(acceptingLibraryId)) Fail(LinkNotRequested);

            RaiseEvent(new LinkCompleted(processId, Id, acceptingLibraryId));
        }

        public void AddBookToLibrary(Guid processId, string title, string author, string isbn, DateTime publishDate)
        {
            if (Books.Contains(new Book(title, author, isbn))) Fail(BookAlreadyInLibrary);
            RaiseEvent(new BookAddedToLibrary(processId, Id, title, author, isbn, publishDate));
        }

        public void RemoveBookFromLibrary(Guid processId, string title, string author, string isbn)
        {
            if (!Books.Contains(new Book(title, author, isbn))) Fail(BookNotInLibrary);
            RaiseEvent(new BookRemovedFromLibrary(processId, Id, title, author, isbn));
        }
    }
}
