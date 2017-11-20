using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Joshilewis.Cqrs;
using Joshilewis.Infrastructure.EventRouting;
using Joshilewis.Testing;
using Joshilewis.Testing.CallBuilders;
using Joshilewis.Testing.Helpers;
using Lending.Domain.AcceptLink;
using Lending.Domain.AddBookToLibrary;
using Lending.Domain.Model;
using Lending.Domain.OpenLibrary;
using Lending.Domain.RemoveBookFromLibrary;
using Lending.Domain.RequestLink;
using Lending.ReadModels.Relational;
using static Joshilewis.Testing.Helpers.ApiExtensions;

namespace Tests
{
    public static class AutomationExtensions
    {
        private static PostCallBuilder command;

        public static Guid OpenLibrary(Guid processId, string userId, string name, string picture)
        {
            command = WhenCommand(new OpenLibrary(processId, userId, name, picture));
            return Guid.Parse(command.IsPOSTedTo<string>("/libraries"));
        }

        public static void LibraryOpenedSuccessfully()
        {
            command.Response.ShouldEqual(new HttpResponseMessage(HttpStatusCode.Created));
        }

        public static void DuplicateEventIgnored()
        {
            command.Response.ShouldEqual(new HttpResponseMessage(HttpStatusCode.OK));
        }

        public static void AddsBookToLibrary(Guid transactionId, Guid libraryId, string userId, string title, string author,
            string isbn, int publishYear)
        {
            command =
                WhenCommand(new AddBookToLibrary(transactionId, libraryId, userId, title, author, isbn,
                    publishYear));
            command.IsPOSTedTo($"/libraries/{libraryId}/books/add");
        }

        public static void RemovesBookFromLibrary(Guid transactionId, Guid libraryId, string userId, string title, string author,
            string isbn, int publishYear)
        {
            command =
                WhenCommand(new RemoveBookFromLibrary(transactionId, libraryId, userId, title, author, isbn,
                    publishYear));
            command.IsPOSTedTo($"/libraries/{libraryId}/books/remove");
        }

        public static void BookAddedSucccessfully()
        {
            command.Response.ShouldEqual(new HttpResponseMessage(HttpStatusCode.OK));
        }

        public static void UnauthorisedCommandIgnored(string userId, Type aggregateType, Guid aggregateId)
        {
            command.Response.ShouldEqual(new HttpResponseMessage(HttpStatusCode.Forbidden)
            {
                ReasonPhrase = $"User {userId} is not authorized for {aggregateType} {aggregateId}"
            });
        }

        public static void BookRemovedSucccessfully()
        {
            command.Response.ShouldEqual(new HttpResponseMessage(HttpStatusCode.OK));
        }

        public static void IgnoreBecauseBookNotInLibrary()
        {
            command.Response.ShouldEqual(new HttpResponseMessage(HttpStatusCode.OK));
        }

        public static void RequestsLibraryLink(Guid transactionId, Guid libraryId, string userId,
            Guid targetLibraryId)
        {
            command =
                WhenCommand(new RequestLink(transactionId, libraryId, userId, targetLibraryId));
            command.IsPOSTedTo($"/libraries/{libraryId}/links/request");
        }

        public static void LinkRequestCreated()
        {
            command.Response.ShouldEqual(new HttpResponseMessage(HttpStatusCode.OK));
        }

        public static void DuplicateLinkRequestIgnored()
        {
            command.Response.ShouldEqual(new HttpResponseMessage(HttpStatusCode.OK));
        }

        public static void FailtNotFound()
        {
            command.Response.ShouldEqual(new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                ReasonPhrase = "Not Found"
            });
        }

        public static void ReverseLinkRequestIgnored()
        {
            command.Response.ShouldEqual(new HttpResponseMessage(HttpStatusCode.OK));

        }

        public static void LinkRequestedToSelfIgnored()
        {
            command.Response.ShouldEqual(new HttpResponseMessage(HttpStatusCode.OK));

        }

        public static void AcceptsLibraryLink(Guid transactionId, Guid libraryId, string userId, Guid requestingLibraryId)
        {
            command =
                WhenCommand(new AcceptLink(transactionId, libraryId, userId, requestingLibraryId));
            command.IsPOSTedTo($"/libraries/{libraryId}/links/accept");
        }
        public static void LinkRequestForLinkedLibrariesIgnored()
        {
            command.Response.ShouldEqual(new HttpResponseMessage(HttpStatusCode.OK));
        }

        public static void LibrariesLinked()
        {
            command.Response.ShouldEqual(new HttpResponseMessage(HttpStatusCode.OK));
        }

        public static void AcceptUnrequestedLinkIgnored()
        {
            command.Response.ShouldEqual(new HttpResponseMessage(HttpStatusCode.OK));
        }

        public static void AcceptLinkForLinkedLibrariesIgnored()
        {
            command.Response.ShouldEqual(new HttpResponseMessage(HttpStatusCode.OK));
        }

        public static void LibraryOpened(Guid processId, string userId, Guid libraryId, string name, string picture)
        {
            HandleEvent(new LibraryOpened(processId, libraryId, name, userId, picture));
        }

        public static void LinkRequested(Guid processId, Guid aggregateId, Guid targetLibraryId)
        {
            HandleEvent(new LinkRequested(processId, aggregateId, targetLibraryId));
        }

        public static void LinkAccepted(Guid processId, Guid aggregateId, Guid requestingLibraryId)
        {
            HandleEvent(new LinkAccepted(processId, aggregateId, requestingLibraryId));
        }

        public static void BookAddedToLibrary(Guid processId, Guid aggregateId, string title, string author, string isbn,
            int publishYear)
        {
            HandleEvent(new BookAddedToLibrary(processId, aggregateId, title, author, isbn, publishYear));
        }

        public static void BookRemovedFromLibrary(Guid processId, Guid aggregateId, string title, string author, string isbn,
            int publishYear)
        {
            HandleEvent(new BookRemovedFromLibrary(processId, aggregateId, title, author, isbn, publishYear));
        }

        public static void HandleEvent(Event @event)
        {
            IEventEmitter eventEmitter = DIExtensions.Container.GetInstance<IEventEmitter>();
            eventEmitter.EmitEvent(@event);

            PersistenceExtensions.OpenTransaction();
            DIExtensions.Container.GetInstance<EventDispatcher>().DispatchEvents();
            PersistenceExtensions.CommitTransaction();
        }

        public static void SearchForLibraries(string searchTerm)
        {
            GetEndpoint("libraries/" + searchTerm);
        }

        public static void SearchForLibrariesAsUser(string searchTerm, string userId)
        {
            GetEndpoint("libraries/" + searchTerm).As(userId);
        }

    }
}
