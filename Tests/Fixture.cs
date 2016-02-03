using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Lending.Cqrs.Command;
using Lending.Cqrs.Query;
using Lending.Execution;
using Lending.Execution.Auth;
using Lending.Execution.DI;
using Microsoft.Owin.Testing;
using NUnit.Framework;
using ServiceStack.ServiceModel.Serialization;
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

        protected PostCallBuilder GivenCommand(AuthenticatedCommand command)
        {
            return new PostCallBuilder(Client, Tokeniser, command, true);
        }

        protected MultiPostCallBuilder GivenCommands(params AuthenticatedCommand[] commands)
        {
            return new MultiPostCallBuilder(Client, Tokeniser, commands);
        }

        private PostCallBuilder whenPostCallBuilder;
        protected PostCallBuilder WhenCommand(AuthenticatedCommand command)
        {
            whenPostCallBuilder = new PostCallBuilder(Client, Tokeniser, command, false);
            return whenPostCallBuilder;
        }

        protected void Then(HttpResponseMessage expectedResponseMessage)
        {
            whenPostCallBuilder.Response.ShouldEqual(expectedResponseMessage);
        }

        protected HttpResponseMessage Http403BecauseUnauthorized(Guid userId, Guid aggregateId, Type aggregateType)
        {
            return new HttpResponseMessage(HttpStatusCode.Forbidden)
            {
                ReasonPhrase = $"User {userId} is not authorized for {aggregateType} {aggregateId}"
            };
        }

        protected string GetResponseString;
        protected Exception ActualException;
        protected void WhenGetEndpoint(string uri, Guid? userId = null)
        {
            try
            {
                GetResponseString = HitEndPoint(uri, userId);
            }
            catch (Exception exception)
            {
                ActualException = exception;
            }
        }

        protected string HitEndPoint(string uri, Guid? userId)
        {
            string path = $"https://localhost/api/{uri}/";

            if (userId.HasValue)
            {
                Client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(Tokeniser.CreateToken("username", userId.Value));
            }
            var response = Client.GetAsync(path).Result;
            return response.Content.ReadAsStringAsync().Result;
        }

        protected void Then<TResult>(Predicate<TResult> resultEqualityPredicate) where TResult : Result
        {
            TResult actualResult = JsonDataContractDeserializer.Instance.DeserializeFromString<TResult>(GetResponseString);
            resultEqualityPredicate(actualResult);
        }

        protected GetCallBuilder<TResult> AndGETTo<TResult>(string url) where TResult : Result
        {
            return new GetCallBuilder<TResult>(Client, Tokeniser, url);
        }

        protected HttpResponseMessage Http400Because(string reasonPhrase)
        {

            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                ReasonPhrase = reasonPhrase
            };
        }
    }
}
