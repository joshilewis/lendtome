using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Lending.Cqrs.Command;
using Lending.Execution;
using Lending.Execution.Auth;
using Lending.Execution.DI;
using Microsoft.Owin.Testing;
using NUnit.Framework;
using StructureMap;

namespace Tests
{
    [TestFixture]
    public abstract class Fixture
    {
        protected IContainer Container;
        protected HttpClient Client;
        protected Tokeniser Tokeniser => Container.GetInstance<Tokeniser>();

        private TestServer server;

        [SetUp]
        public virtual void SetUp()
        {
            Container = IoC.Initialize(new TestRegistry());

            server = TestServer.Create<Startup>();
            Client = server.HttpClient;
        }

        [TearDown]
        public virtual void TearDown()
        {
            server.Dispose();
        }

        protected void Given(AuthenticatedCommand command, string url)
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Tokeniser.CreateToken("username", command.UserId));
            HttpResponseMessage response = Client.PostAsJsonAsync($"https://localhost/api/{url}", command).Result;
            if (!response.IsSuccessStatusCode) throw new AssertionException("Given HTTP request resulted in an error");
        }

        private PostBuilder whenPostBuilder;
        protected PostBuilder WhenCommand(AuthenticatedCommand command)
        {
            whenPostBuilder = new PostBuilder(command, Client, Tokeniser);
            return whenPostBuilder;
        }

        protected void Then(HttpResponseMessage expectedResponseMessage)
        {
            whenPostBuilder.Response.ShouldEqual(expectedResponseMessage);
        }

        protected HttpResponseMessage Http403BecauseUnauthorized(Guid userId, Guid aggregateId, Type aggregateType)
        {
            return new HttpResponseMessage(HttpStatusCode.Forbidden)
            {
                ReasonPhrase = $"User {userId} is not authorized for {aggregateType} {aggregateId}"
            };
        }


    }
}
