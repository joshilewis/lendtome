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

        private readonly List<CallBuilder> givenCalls = new List<CallBuilder>();

        protected CallBuilder GivenCommand(AuthenticatedCommand command)
        {
            var callBuilder = new CallBuilder(command, Client, Tokeniser);
            givenCalls.Add(callBuilder);
            return callBuilder;
        }

        private CallBuilder whenCallBuilder;
        protected CallBuilder WhenCommand(AuthenticatedCommand command)
        {
            if (givenCalls.Any(x => x.Exception != null))
            {
                throw new AssertionException(
                    $"The following Given calls failed: {givenCalls.Where(x => x.Exception != null).Select(x => $"{x.Command} to {x.Url}")}");
            }

            whenCallBuilder = new CallBuilder(command, Client, Tokeniser);
            return whenCallBuilder;
        }

        protected void Then(HttpResponseMessage expectedResponseMessage)
        {
            whenCallBuilder.Response.ShouldEqual(expectedResponseMessage);
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
        protected void WhenGetEndpoint(string uri)
        {
            try
            {
                GetResponseString = HitEndPoint(uri);
            }
            catch (Exception exception)
            {
                ActualException = exception;
            }
        }

        protected string HitEndPoint(string uri)
        {
            string path = $"https://localhost/api/{uri}/";
            var response = Client.GetAsync(path).Result;
            return response.Content.ReadAsStringAsync().Result;
        }

        protected void Then<TResult>(Result expectedResult) where TResult : Result
        {
            TResult actualResult = JsonDataContractDeserializer.Instance.DeserializeFromString<TResult>(GetResponseString);
            actualResult.ShouldEqual(expectedResult);
        }

        protected GetCallBuilder<TResult> AndGETTo<TResult>(string url) where TResult : Result
        {
            return new GetCallBuilder<TResult>(url, Client, Tokeniser);
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
