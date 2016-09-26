﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Lending.Domain.AcceptLink;
using Lending.Domain.AddBookToLibrary;
using Lending.Domain.OpenLibrary;
using Lending.Domain.RemoveBookFromLibrary;
using Lending.Domain.RequestLink;
using Lending.ReadModels.Relational;
using Lending.ReadModels.Relational.LibraryOpened;
using Lending.ReadModels.Relational.ListLibrayLinks;

namespace Tests
{
    public class TestData
    {
        public static Guid ProcessId = Guid.Empty;
        public static Guid Library1Id = Guid.NewGuid();
        public static Guid Library2Id = Guid.NewGuid();
        public static Guid Library3Id = Guid.NewGuid();
        public static Guid Library4Id = Guid.NewGuid();
        public static Guid Library5Id = Guid.NewGuid();
        public static Guid Library6Id = Guid.NewGuid();

        public static string Library1Name = "library1";
        public static string Library2Name = "library2";
        public static string Library3Name = "library3";
        public static string Library4Name = "library4";
        public static string Library5Name = "library5";
        public static string Library6Name = "library6";

        public static string Library1Picture = "libraryPicture1";
        public static string Library2Picture = "libraryPicture2";
        public static string Library3Picture = "libraryPicture3";
        public static string Library4Picture = "libraryPicture4";
        public static string Library5Picture = "libraryPicture5";
        public static string Library6Picture = "libraryPicture6";

        public static HttpResponseMessage Http201Created = new HttpResponseMessage(HttpStatusCode.Created);
        public static HttpResponseMessage Http200Ok = new HttpResponseMessage(HttpStatusCode.OK);

        public static AuthenticatedUser User1
            =>
                new AuthenticatedUser(Library1Id, Library1Name, "email1", Library1Picture,
                    new List<AuthenticationProvider>());

        public static AuthenticatedUser User2
            =>
                new AuthenticatedUser(Library2Id, Library2Name, "email2", Library2Picture,
                    new List<AuthenticationProvider>());

        public static AuthenticatedUser User3
            =>
                new AuthenticatedUser(Library3Id, Library3Name, "email3", Library3Picture,
                    new List<AuthenticationProvider>());

        public static AuthenticatedUser User4
            =>
                new AuthenticatedUser(Library4Id, Library4Name, "email4", Library4Picture,
                    new List<AuthenticationProvider>());
        public static AuthenticatedUser User5
            =>
                new AuthenticatedUser(Library5Id, Library5Name, "email5", Library5Picture,
                    new List<AuthenticationProvider>());
        public static AuthenticatedUser User6
            =>
                new AuthenticatedUser(Library6Id, Library6Name, "email6", Library6Picture,
                    new List<AuthenticationProvider>());

        public static OpenLibrary OpenLibrary1 = new OpenLibrary(ProcessId, Library1Id, Library1Id, Library1Name);
        public static OpenLibrary OpenLibrary2 = new OpenLibrary(ProcessId, Library2Id, Library2Id, Library2Name);
        public static OpenLibrary OpenLibrary3 = new OpenLibrary(ProcessId, Library3Id, Library3Id, Library3Name);
        public static OpenLibrary OpenLibrary4 = new OpenLibrary(ProcessId, Library4Id, Library4Id, Library4Name);
        public static OpenLibrary OpenLibrary5 = new OpenLibrary(ProcessId, Library5Id, Library5Id, Library5Name);
        public static OpenLibrary OpenLibrary6 = new OpenLibrary(ProcessId, Library6Id, Library6Id, Library6Name);

        public static string Title = "Title";
        public static string Author = "Author";
        public static string Isbnnumber = "isbn";
        public static int PublishYear = 2000;

        public static AddBookToLibrary AddBook1ToLibrary = new AddBookToLibrary(ProcessId, Guid.Empty, Library1Id, Title,
            Author, Isbnnumber, PublishYear);

        public static AddBookToLibrary UnauthorizedAddBookToLibrary = new AddBookToLibrary(ProcessId, Guid.Empty, Guid.Empty, Title,
            Author, Isbnnumber, PublishYear);

        public static BookAddedToLibrary Book1AddedToUser1Library = new BookAddedToLibrary(ProcessId, Library1Id, Title,
            Author, Isbnnumber, PublishYear);

        public static RemoveBookFromLibrary User1RemovesBookFromLibrary = new RemoveBookFromLibrary(ProcessId, Guid.Empty,
            Library1Id, Title, Author, Isbnnumber, PublishYear);

        public static RemoveBookFromLibrary UnauthorizedRemoveBook = new RemoveBookFromLibrary(ProcessId, Guid.Empty,
            Guid.Empty, Title, Author, Isbnnumber, PublishYear);

        public static BookRemovedFromLibrary Book1RemovedFromLibrary = new BookRemovedFromLibrary(ProcessId, Library1Id,
            Title, Author, Isbnnumber, PublishYear);

        public static RequestLink Library1RequestsLinkToLibrary3 = new RequestLink(ProcessId, Guid.Empty,
            Library1Id, Library3Id);
        public static RequestLink Library1RequestsLinkToLibrary4 = new RequestLink(ProcessId, Guid.Empty,
            Library1Id, Library4Id);
        public static RequestLink Library1RequestsLinkToLibrary5 = new RequestLink(ProcessId, Guid.Empty,
            Library1Id, Library5Id);
        public static RequestLink Library1RequestsLinkToLibrary6 = new RequestLink(ProcessId, Guid.Empty,
            Library1Id, Library6Id);

        public static AcceptLink Library3AcceptsLinkFromLibrary1 = new AcceptLink(ProcessId, Guid.Empty, Library3Id,
            Library1Id);
        public static AcceptLink Library4AcceptsLinkFromLibrary1 = new AcceptLink(ProcessId, Guid.Empty, Library4Id,
            Library1Id);
        public static AcceptLink Library5AcceptsLinkFromLibrary1 = new AcceptLink(ProcessId, Guid.Empty, Library5Id,
            Library1Id);
        public static AcceptLink Library6AcceptsLinkFromLibrary1 = new AcceptLink(ProcessId, Guid.Empty, Library6Id,
            Library1Id);

        public static AcceptLink UnauthorizedAcceptLink = new AcceptLink(ProcessId, Guid.Empty, Guid.Empty, Library1Id);

        public static LinkCompleted LinkCompleted = new LinkCompleted(ProcessId, Library1Id, Library2Id);
        public static LinkAccepted LinkAccepted = new LinkAccepted(ProcessId, Library2Id, Library1Id);

        public static HttpResponseMessage Http404BecauseTargetLibraryDoesNotExist =
            new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                //ReasonPhrase = $"Aggregate '{Library2Id}' (type {typeof (Library).Name}) was not found.",
                ReasonPhrase = "Not Found"
            };

        public static RequestLink Library2RequestsLinkToLibrary1 = new RequestLink(ProcessId, Guid.Empty,
            Library2Id, Library1Id);

        public static RequestLink Library1Requests2NdLinkToLibrary2 = new RequestLink(ProcessId, Guid.Empty,
            Library1Id, Library2Id);

        public static LinkRequested LinkRequestedFrom2To1 = new LinkRequested(ProcessId, Library2Id,
            Library1Id);

        public static LinkRequestReceived LinkRequestFrom2To1Received =
            new LinkRequestReceived(ProcessId, Library1Id, Library2Id);

        public static RequestLink Library1RequestsLinkToSelf = new RequestLink(ProcessId, Guid.Empty,
            Library1Id, Library1Id);

        public static RequestLink UnauthorizedRequestLink = new RequestLink(ProcessId, Guid.Empty,
            Guid.Empty, Library2Id);

        public static OpenLibrary JoshuaLewisOpensLibrary = new OpenLibrary(ProcessId, Library1Id, Library1Id,
            "Joshua Lewis");
        public static OpenLibrary SuzaanHepburnOpensLibrary = new OpenLibrary(ProcessId, Library2Id, Library2Id, "Suzaan Hepburn");
        public static OpenLibrary JosieDoeOpensLibrary = new OpenLibrary(ProcessId, Library3Id, Library3Id, "Josie Doe");
        public static OpenLibrary AudreyHepburnOpensLibrary = new OpenLibrary(ProcessId, Library4Id, Library4Id, "Audrey Hepburn");

        public static LibraryOpened JoshuaLewisLibraryOpened = new LibraryOpened(ProcessId, Library1Id, "Joshua Lewis", Library1Id);
        public static LibraryOpened SuzaanHepburnLibraryOpened = new LibraryOpened(ProcessId, Library2Id, "Suzaan Hepburn",
            Library2Id);
        public static LibraryOpened JosieDoeLibraryOpened = new LibraryOpened(ProcessId, Library3Id, "Josie Doe", Library3Id);
        public static LibraryOpened AudreyHepburnLibraryOpened = new LibraryOpened(ProcessId, Library4Id,
            "Audrey Hepburn", Library4Id);

        public static string TestDrivenDevelopment = "Test-Driven Development";
        public static string KentBeck = "Kent Beck";
        public static string Isbn = "Isbn";
        public static string ExtremeProgrammingExplained = "Extreme Programming Explained";
        public static string ExtremeSnowboardStunts = "Extreme Snowboard Stunts";
        public static string SomeSkiier = "Some Skiier";
        public static string BeckAMusicalMaestro = "Beck: A musical Maestro";
        public static string SomeAuthor = "Some Author";

        public static LibraryOpened Library3Opened = new LibraryOpened(ProcessId, Library3Id, "User3", Library3Id);
        public static LibraryOpened Library4Opened = new LibraryOpened(ProcessId, Library4Id, "User4", Library4Id);
        public static LibraryOpened Library5Opened = new LibraryOpened(ProcessId, Library5Id, "User5", Library5Id);
        public static LibraryOpened Library6Opened = new LibraryOpened(ProcessId, Library6Id, "User6", Library6Id);

        public static LinkAccepted Link1To2Accepted = new LinkAccepted(ProcessId, Library2Id, Library1Id);
        public static LinkAccepted Link1To3Accepted = new LinkAccepted(ProcessId, Library3Id, Library1Id);
        public static LinkAccepted Link1To4Accepted = new LinkAccepted(ProcessId, Library4Id, Library1Id);
        public static LinkAccepted Link1To5Accepted = new LinkAccepted(ProcessId, Library5Id, Library1Id);
        public static LinkAccepted Link1To6Accepted = new LinkAccepted(ProcessId, Library6Id, Library1Id);

        public static AddBookToLibrary Lib2AddsXpeByKb = new AddBookToLibrary(ProcessId, Library2Id, Library2Id,
            ExtremeProgrammingExplained, KentBeck, Isbn, PublishYear);
        public static AddBookToLibrary Lib3AddsTddByKb = new AddBookToLibrary(ProcessId, Library3Id, Library3Id,
            TestDrivenDevelopment, KentBeck, Isbn, PublishYear);
        public static AddBookToLibrary Lib4AddsXpeByKb = new AddBookToLibrary(ProcessId, Library4Id, Library4Id,
            ExtremeProgrammingExplained, KentBeck, Isbn, PublishYear);
        public static AddBookToLibrary Lib5AddsEssBySs = new AddBookToLibrary(ProcessId, Library5Id, Library5Id,
            ExtremeSnowboardStunts, SomeSkiier, Isbn, PublishYear);
        public static AddBookToLibrary Lib6AddsBBySA = new AddBookToLibrary(ProcessId, Library6Id, Library6Id,
            BeckAMusicalMaestro, SomeAuthor, Isbn, PublishYear);
        public static RemoveBookFromLibrary Lib4RemovesXpeByKb = new RemoveBookFromLibrary(ProcessId, Library4Id, Library4Id,
            ExtremeProgrammingExplained, KentBeck, Isbn, PublishYear);

        public static LibrarySearchResult[] EmptyList = { };

    }
}
