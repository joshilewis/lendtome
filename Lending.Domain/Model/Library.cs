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
        public const string NoLinkRequested = "This library did not receive a link request from the specified library";
        public const string LinkAlreadyRequested = "A link request for these libraries already exists";
        public const string ReverseLinkAlreadyRequested = "A reverse link request for these libraries already exists";
        public const string LinkNotRequested = "This library did not request to link with the specified library";

        public string Name { get; protected set; }
        public List<AdministratorId> Administrators { get; }

        public List<LibraryId> SentLinkRequests { get; }
        public List<LibraryId> ReceivedLinkRequests { get; }
        public List<LibraryId> LinkedLibraries { get; }
        public List<Book> Books { get; } 

        protected Library(Guid processId, LibraryId id, string name, AdministratorId administratorId)
            : this()
        {
            RaiseEvent(new LibraryOpened(processId, id.Id, name, administratorId.Id));
        }

        protected Library()
        {
            Administrators = new List<AdministratorId>();
            SentLinkRequests = new List<LibraryId>();
            ReceivedLinkRequests = new List<LibraryId>();
            LinkedLibraries = new List<LibraryId>();
            Books = new List<Book>();
        }

        public static Library Open(Guid processId, LibraryId id, string name, AdministratorId adminId)
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
            Administrators.Add(new AdministratorId(@event.AdministratorId));
        }

        protected virtual void When(LinkRequested @event)
        {
            SentLinkRequests.Add(new LibraryId(@event.TargetLibraryId));
        }

        protected virtual void When(LinkRequestReceived @event)
        {
            ReceivedLinkRequests.Add(new LibraryId(@event.RequestingLibraryId));
        }

        protected virtual void When(LinkAccepted @event)
        {
            ReceivedLinkRequests.Remove(new LibraryId(@event.RequestingLibraryId));
            LinkedLibraries.Add(new LibraryId(@event.RequestingLibraryId));
        }

        protected virtual void When(LinkCompleted @event)
        {
            SentLinkRequests.Remove(new LibraryId(@event.AcceptingLibraryId));
            LinkedLibraries.Add(new LibraryId(@event.AcceptingLibraryId));
        }

        protected virtual void When(BookAddedToLibrary @event)
        {
            Books.Add(new Book(@event.Title, @event.Author, @event.Isbn, @event.PublishYear));
        }

        protected virtual void When(BookRemovedFromLibrary @event)
        {
            Books.Remove(new Book(@event.Title, @event.Author, @event.Isbn, @event.PublishYear));
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

        public void CheckUserAuthorized(AdministratorId administratorId)
        {
            if (!Administrators.Contains(administratorId)) Fail(administratorId);
        }

        protected virtual void Fail(AdministratorId administratorId)
        {
            throw new NotAuthorizedException(administratorId.Id, Id, GetType());
        }

        public void RequestLink(Guid processId, LibraryId desinationLibraryId)
        { 
            if (SentLinkRequests.Contains(desinationLibraryId)) return;
            if (ReceivedLinkRequests.Contains(desinationLibraryId)) return;
            if(LinkedLibraries.Contains(desinationLibraryId)) return;

            RaiseEvent(new LinkRequested(processId, Id, desinationLibraryId.Id));
        }

        public void ReceiveLinkRequest(Guid processId, LibraryId sourceLibraryId)
        {
            if (ReceivedLinkRequests.Contains(sourceLibraryId)) return;
            if (SentLinkRequests.Contains(sourceLibraryId)) return;
            if (LinkedLibraries.Contains(sourceLibraryId)) return;

            RaiseEvent(new LinkRequestReceived(processId, Id, sourceLibraryId.Id));
        }

        public void AcceptLink(Guid processId, LibraryId requestingLibraryId)
        {
            if (LinkedLibraries.Contains(requestingLibraryId)) return;
            if (!ReceivedLinkRequests.Contains(requestingLibraryId)) return;

            RaiseEvent(new LinkAccepted(processId, Id, requestingLibraryId.Id));
        }

        public void CompleteLink(Guid processId, LibraryId acceptingLibraryId)
        {
            if (LinkedLibraries.Contains(acceptingLibraryId)) return;
            if (!SentLinkRequests.Contains(acceptingLibraryId)) return;

            RaiseEvent(new LinkCompleted(processId, Id, acceptingLibraryId.Id));
        }

        public void AddBookToLibrary(Guid processId, string title, string author, string isbn, int publishYear)
        {
            if (Books.Contains(new Book(title, author, isbn, publishYear))) return;
            RaiseEvent(new BookAddedToLibrary(processId, Id, title, author, isbn, publishYear));
        }

        public void RemoveBookFromLibrary(Guid processId, string title, string author, string isbn, int publishYear)
        {
            if (!Books.Contains(new Book(title, author, isbn, publishYear))) return;
            RaiseEvent(new BookRemovedFromLibrary(processId, Id, title, author, isbn, publishYear));
        }
    }
}
