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

        protected void Given(AuthenticatedCommand command, string url)
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Tokeniser.CreateToken("username", command.UserId));
            HttpResponseMessage response = Client.PutAsJsonAsync($"https://localhost/api/{url}", command).Result;
            if (!response.IsSuccessStatusCode) throw new AssertionException("Given HTTP request resulted in an error");
        }

        private CallBuilder whenCallBuilder;
        protected CallBuilder WhenCommand(AuthenticatedCommand command)
        {
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
    }
}
