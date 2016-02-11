using System;
using System.Net;
using System.Net.Http;
using Lending.Cqrs.Command;
using Lending.Cqrs.Query;
using Lending.Execution.Auth;
using Microsoft.Owin.Testing;

namespace Tests
{
    public static class FixtureApiExtensions
    {
        private static TestServer server;
        private static HttpClient client;

        public static void SetUpOwinServer<TStartup>(this Fixture fixture)
        {
            server = TestServer.Create<TStartup>();
            client = server.HttpClient;
        }

        public static void TearDownOwinServer(this Fixture fixture)
        {
            server.Dispose();
        }

        public static HttpClient GetHttpClient(this Fixture fixture)
        {
            return client;
        }

        public static Tokeniser GetTokeniser(this Fixture fixture)
        {
            return fixture.GetContainer().GetInstance<Tokeniser>();
        }

        public static PostCallBuilder GivenCommand(this Fixture fixture, AuthenticatedCommand command)
        {
            return new PostCallBuilder(GetHttpClient(fixture), GetTokeniser(fixture), command, true);
        }

        public static MultiPostCallBuilder GivenCommands(this Fixture fixture, params AuthenticatedCommand[] commands)
        {
            return new MultiPostCallBuilder(GetHttpClient(fixture), GetTokeniser(fixture), commands);
        }

        private static PostCallBuilder whenPostCallBuilder;
        public static PostCallBuilder WhenCommand(this Fixture fixture, AuthenticatedCommand command)
        {
            whenPostCallBuilder = new PostCallBuilder(GetHttpClient(fixture), GetTokeniser(fixture), command, false);
            return whenPostCallBuilder;
        }

        public static void Then(this Fixture fixture, HttpResponseMessage expectedResponseMessage)
        {
            whenPostCallBuilder.Response.ShouldEqual(expectedResponseMessage);
        }

        public static HttpResponseMessage Http403BecauseUnauthorized(this Fixture fixture, Guid userId, Guid aggregateId, Type aggregateType)
        {
            return new HttpResponseMessage(HttpStatusCode.Forbidden)
            {
                ReasonPhrase = $"User {userId} is not authorized for {aggregateType} {aggregateId}"
            };
        }

        private static GetCallBuilder whenGetCallBuilder;

        public static GetCallBuilder WhenGetEndpoint(this Fixture fixture, string uri)
        {
            whenGetCallBuilder = new GetCallBuilder(fixture.GetHttpClient(), fixture.GetTokeniser(), uri);
            return whenGetCallBuilder;
        }

        public static void ThenResponseIs<TPayload>(this Fixture fixture, TPayload expected)
        {
            whenGetCallBuilder.Returns(expected);
        }

        public static GetCallBuilder AndGETTo(this Fixture fixture, string url)
        {
            return new GetCallBuilder(fixture.GetHttpClient(), fixture.GetTokeniser(), url);
        }

        public static HttpResponseMessage Http400Because(this Fixture fixture, string reasonPhrase)
        {

            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                ReasonPhrase = reasonPhrase
            };
        }
    }
}